using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Tag = EShop.Domain.Entities.Tag;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoOrderRepository(MongoDbContext mongoDb) :
        MongoGenericRepository<MongoOrder>(mongoDb, MongoCollectionsName.Order), IMongoOrderRepository
    {
        private readonly IMongoCollection<MongoOrder> _order =
            mongoDb.GetCollection<MongoOrder>(MongoCollectionsName.Order);

        public async Task<MongoOrder> FindUserUnpaidOrderAsync(long userId)
        {
            return await _order.Find(x => x.UserId == userId && !x.IsPayed).SingleOrDefaultAsync();
        }
    }
}