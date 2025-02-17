using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoColorRepository(MongoDbContext mongoDb)
        : MongoGenericRepository<MongoColor>(mongoDb, MongoCollectionsName.Color), IMongoColorRepository
    {
        private readonly IMongoCollection<MongoColor> _color =
            mongoDb.GetCollection<MongoColor>(MongoCollectionsName.Color);

        public async Task<List<MongoColor>> GetAllColorsByIdAsync(List<long> ids)
        {
            return await MongoQueryable.ToListAsync(_color.AsQueryable().Where(x => ids.Contains(x.Id)));
        }
    }
}