using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Tag = EShop.Domain.Entities.Tag;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoCategoryFeatureRepository(MongoDbContext mongoDb) : IMongoCategoryFeatureRepository
    {
        private readonly IMongoCollection<CategoryFeature> _categoryFeature = mongoDb.GetCollection<CategoryFeature>();


        public async Task CreateAsync(CategoryFeature categoryFeature)
        {
            await _categoryFeature.InsertOneAsync(categoryFeature);
        }

        public async Task Delete(CategoryFeature categoryFeature)
        {
            var filter = Builders<CategoryFeature>.Filter.Eq(x=>x.CategoryId,categoryFeature.CategoryId);
            await _categoryFeature.DeleteOneAsync(filter);
        }
    }
}