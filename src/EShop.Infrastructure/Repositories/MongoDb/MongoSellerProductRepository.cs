using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Product.Requests.Queries;
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


        public async Task CreateAsync(MongoSellerProduct entity)
        {
            await _sellerProduct.InsertOneAsync(entity);
        }

        public async Task Update(MongoSellerProduct entity)
        {
            var filter = Builders<MongoSellerProduct>.Filter.Eq(x => x.SellerId, entity.SellerId)
                & Builders<MongoSellerProduct>.Filter.Eq(x => x.ProductId, entity.ProductId);
            
            await _sellerProduct.ReplaceOneAsync(filter, entity);
        }

        public async Task Delete(MongoSellerProduct entity)
        {
            var filter = Builders<MongoSellerProduct>.Filter.Eq(x=>x.SellerId,entity.SellerId)
                         & Builders<MongoSellerProduct>.Filter.Eq(x => x.ProductId, entity.ProductId);

            await _sellerProduct.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<MongoSellerProduct>> GetAllBySellerIdAsync(long sellerId)
        {
            return await _sellerProduct.Find(x=>x.SellerId==sellerId).ToListAsync();
        }

        public async Task<IEnumerable<MongoSellerProduct>> GetAllByProductIdAsync(long productId)
        {
            return await _sellerProduct.Find(x=>x.ProductId==productId).ToListAsync();
        }
    }
}