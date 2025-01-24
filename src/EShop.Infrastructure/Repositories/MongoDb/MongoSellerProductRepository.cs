using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;
using EShop.Application.Features.SellerPanel.Requests.Queries;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoSellerProductRepository(MongoDbContext mongoDb) : IMongoSellerProductRepository
    {
        private readonly IMongoCollection<MongoSellerProduct> _sellerProduct =
            mongoDb.GetCollection<MongoSellerProduct>(MongoCollectionsName.SellerProduct);
        private readonly IMongoCollection<MongoSeller> _seller =
            mongoDb.GetCollection<MongoSeller>(MongoCollectionsName.Seller);

        private readonly IMongoCollection<MongoProduct> _product =
            mongoDb.GetCollection<MongoProduct>(MongoCollectionsName.Product);


        public async Task CreateAsync(MongoSellerProduct entity)
        {
            await _sellerProduct.InsertOneAsync(entity);
        }

        public async Task CreateAllAsync(List<MongoSellerProduct> entities)
        {
            await _sellerProduct.InsertManyAsync(entities);
        }

        public async Task Update(MongoSellerProduct entity)
        {
            var filter = Builders<MongoSellerProduct>.Filter.Eq(x => x.Id, entity.Id);

            await _sellerProduct.ReplaceOneAsync(filter, entity);
        }

        public async Task Delete(MongoSellerProduct entity)
        {
            var filter = Builders<MongoSellerProduct>.Filter.Eq(x => x.Id, entity.Id);

            await _sellerProduct.DeleteOneAsync(filter);
        }

        public async Task<List<MongoSellerProduct>> GetAllBySellerIdAsync(long sellerId)
        {
            return await _sellerProduct.Find(x => x.SellerId == sellerId).ToListAsync();
        }

        public async Task<List<GetSellersProductDto>> GetAllByProductAndColorIdAsync(long productId,long colorId)
        {
            var sellerProductIQueryable = from sellerProduct in _sellerProduct.AsQueryable()
                    .Where(x => x.ProductId == productId)
                join seller in _seller on sellerProduct.SellerId equals seller.Id
                select new GetSellersProductDto()
                {
                    BasePrice = sellerProduct.BasePrice,
                    ColorId = sellerProduct.ColorId,
                    Count = sellerProduct.Count,
                    DiscountPercentage = sellerProduct.DiscountPercentage,
                    SellerId = sellerProduct.SellerId,
                    EndOfDiscount = sellerProduct.EndOfDiscount,
                    ShopName = seller.ShopName
                };
             var results= await MongoQueryable.ToListAsync(sellerProductIQueryable);
             foreach (var result in results.Where(result => result.DiscountPercentage>0))
             {
                 result.PriceWithDiscount=MathHelper.CalculatePriceWithDiscount(result.BasePrice, result.DiscountPercentage);
             }

             return results;
        }

        public async Task<MongoSellerProduct?> FindReserveAsync(long productId, long colorId,long sellerId)
        {
            return  await MongoQueryable.SingleOrDefaultAsync( _sellerProduct.AsQueryable()
                .Where(x=>x.ProductId==productId && x.ColorId==colorId&& x.SellerId==sellerId));
        }

        public async Task<GetAllReservedProductsQueryResponse> GetAllReservedProductsAsync(
            SearchSellerProductDto search, long sellerId)
        {
            var sellerProductQuery = _sellerProduct.AsQueryable().Where(x => x.SellerId == sellerId);
            if (!sellerProductQuery.Any())
            {
                return new GetAllReservedProductsQueryResponse([], search, 0);
            }

            #region Search

            sellerProductQuery =
                sellerProductQuery.CreateContainsExpression("Product.Title", search.Title);

            if (search.CategoryId > 0)
            {
                sellerProductQuery = sellerProductQuery
                    .Where(x => x.Product.CategoryId == search.CategoryId);
            }

            #endregion

            #region Sort

            sellerProductQuery =
                sellerProductQuery.CreateOrderByExpression(search.SortingBy.ToString(), search.SortingAs);

            #endregion

            #region Paging

            ( IQueryable<MongoSellerProduct> query, int pageCount) pagination =
                sellerProductQuery.Page(search.Pagination.CurrentPage, search.Pagination.TakeRecord);
            sellerProductQuery = pagination.query;

            #endregion

            var products = await MongoQueryable.ToListAsync(sellerProductQuery.Select
            (x => new ShowAllReservedProductDto
            {
                ProductId = x.ProductId,
                Title = x.Product.Title,
                Image = x.Product.Images.First(),
                ColorId = x.ColorId,
                Count = x.Count,
                BasePrice = x.BasePrice
            }));
            return new GetAllReservedProductsQueryResponse(products, search, pagination.pageCount);
        }
    }
}