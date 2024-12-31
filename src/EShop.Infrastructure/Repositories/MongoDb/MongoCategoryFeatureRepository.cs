using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoCategoryFeatureRepository(MongoDbContext mongoDb) : IMongoCategoryFeatureRepository
    {
        private readonly IMongoCollection<MongoCategoryFeature> _categoryFeature =
            mongoDb.GetCollection<MongoCategoryFeature>(MongoCollectionsName.CategoryFeature);


        public async Task CreateAsync(MongoCategoryFeature categoryFeature)
        {
            await _categoryFeature.InsertOneAsync(categoryFeature);
        }

        public async Task Delete(MongoCategoryFeature categoryFeature)
        {
            var filter = Builders<MongoCategoryFeature>.Filter.Eq(x=>x.CategoryId,categoryFeature.CategoryId);
            await _categoryFeature.DeleteOneAsync(filter);
        }
    }
}