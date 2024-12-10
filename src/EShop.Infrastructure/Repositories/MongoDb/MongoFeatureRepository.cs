using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Tag = EShop.Domain.Entities.Tag;

namespace EShop.Infrastructure.Repositories.MongoDb;

    public class MongoFeatureRepository(MongoDbContext mongoDb) : MongoGenericRepository<Feature>(mongoDb), IMongoFeatureRepository
    {
    }
