using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoProvinceRepository(MongoDbContext mongoDb) : MongoGenericRepository<MongoProvince>(mongoDb,MongoCollectionsName.Province), IMongoProvinceRepository
    {
    }
}