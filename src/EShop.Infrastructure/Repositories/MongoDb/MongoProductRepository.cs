using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Product.Requests.Queries;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoProductRepository(MongoDbContext mongoDb)
        : MongoGenericRepository<MongoProduct>(mongoDb, MongoCollectionsName.Product),
            IMongoProductRepository
    {
        private readonly IMongoCollection<MongoProduct> _product =
            mongoDb.GetCollection<MongoProduct>(MongoCollectionsName.Product);

        private readonly IMongoCollection<MongoSellerProduct> _sellerProduct =
            mongoDb.GetCollection<MongoSellerProduct>(MongoCollectionsName.SellerProduct);

        public async Task<GetAllProductQueryResponse> GetAllAsync(SearchProductDto search)
        {
            var productQuery = _product
                .AsQueryable().IgnoreQueryFilters();

            #region Search

            productQuery = productQuery.CreateContainsExpression(nameof(Product.Title), search.Title);
            productQuery = productQuery.CreateContainsExpression(nameof(Product.EnglishTitle), search.EnglishTitle);

            if (search.BasePrice > 0)
            {
                productQuery = productQuery.Where(x => x.BasePrice == search.BasePrice);
            }

            if (!string.IsNullOrWhiteSpace(search.CategoryTitle))
            {
                productQuery = productQuery.Where(x => x.CategoryTitle == search.CategoryTitle);
            }

            #endregion

            #region Sort

            productQuery = productQuery.CreateOrderByExperssion(search.SortingBy.ToString(), search.SortingAs);

            productQuery = productQuery.CreateDeleteStatusExperssion(nameof(BaseEntity.IsDelete), search.DeleteStatus);

            #endregion

            #region Paging

            (IQueryable<MongoProduct> query, int pageCount) pagination =
                productQuery.Page(search.Pagination.CurrentPage, search.Pagination.TakeRecord);
            productQuery = pagination.query;

            #endregion

            var sellerProductQuery = _sellerProduct.AsQueryable();
            var productsId = await MongoQueryable.ToListAsync(sellerProductQuery
                .Select(x => x.ProductId));
            var totalCount = await MongoQueryable.SumAsync(sellerProductQuery
                .Where(x => productsId.Contains(x.ProductId)).Select(x => x.Count));

            var products = await MongoQueryable.ToListAsync(productQuery.Select(x =>
                new ShowAllProductDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    EnglishTitle = x.EnglishTitle,
                    CategoryTitle = x.CategoryTitle,
                    DiscountPercentage = x.DiscountPercentage,
                    BasePrice = x.BasePrice,
                    Count = totalCount,
                    Image = x.Images.First(),
                }));
            foreach (var product in products.Where(product => product.DiscountPercentage > 0))
            {
                product.PriceWithDiscount = MathHelper.CalculatePriceWithDiscount(product.BasePrice, product.DiscountPercentage);
            }

            return new GetAllProductQueryResponse(products, search, pagination.pageCount);
        }

        public async Task<int> CountProductByIdAsync(long productId)
        {
            return await MongoQueryable.SumAsync(_sellerProduct.AsQueryable()
                .Where(x => x.ProductId == productId).Select(x => x.Count));
        }
    }
}