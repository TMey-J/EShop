using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Tag = EShop.Domain.Entities.Tag;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoColorRepository(MongoDbContext mongoDb) : MongoGenericRepository<Color>(mongoDb), IMongoColorRepository
    {
        private readonly IMongoCollection<Color> _color = mongoDb.GetCollection<Color>();

        
    }
}