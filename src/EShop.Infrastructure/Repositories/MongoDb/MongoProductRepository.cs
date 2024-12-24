using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoProductRepository(MongoDbContext mongoDb) : MongoGenericRepository<ReadProduct>(mongoDb,MongoCollectionsName.Product),
        IMongoProductRepository
    {
        private readonly IMongoCollection<ReadProduct> _product = mongoDb.GetCollection<ReadProduct>(MongoCollectionsName.Product);
    }
}