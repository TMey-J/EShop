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

            var products = await MongoQueryable.ToListAsync(productQuery.Select(x =>
                new ShowAllProductDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    EnglishTitle = x.EnglishTitle,
                    CategoryTitle = x.CategoryTitle,
                    Image = x.Images.First(),
                }));

            return new GetAllProductQueryResponse(products, search, pagination.pageCount);
        }

        public async Task<int> CountProductByIdAsync(long productId)
        {
            return await MongoQueryable.SumAsync(_sellerProduct.AsQueryable()
                .Where(x => x.ProductId == productId).Select(x => (int)x.Count));
        }

        public async Task<List<long>> GetProductColorsIdAsync(long productId)
        {
            return await MongoQueryable.ToListAsync(_sellerProduct.AsQueryable().Where(x => x.ProductId == productId)
                .Select(x => x.ColorId));
        }

        public async Task<Dictionary<string, string>> GetProductFeaturesAsync(long productId)
        {
            return await MongoQueryable.FirstAsync(_product.AsQueryable()
                .Where(x => x.Id == productId)
                .Select(x => x.Features));
        }

        public async Task<List<MongoProduct>> SearchProductByTitleAsync(string title,CancellationToken cancellationToken)
        {
            return await MongoQueryable.ToListAsync(
                _product.AsQueryable()
                    .Where(x => x.Title.Contains(title, StringComparison.CurrentCultureIgnoreCase) ||
                                x.EnglishTitle.Contains(title, StringComparison.CurrentCultureIgnoreCase)), cancellationToken);
        }
    }
}