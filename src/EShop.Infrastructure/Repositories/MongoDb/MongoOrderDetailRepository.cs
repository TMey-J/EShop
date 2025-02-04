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
    public class MongoOrderDetailRepository(MongoDbContext mongoDb) :
        MongoGenericRepository<MongoOrderDetail>(mongoDb,MongoCollectionsName.OrderDetail), IMongoOrderDetailRepository
    {
        private readonly IMongoCollection<MongoOrderDetail> _orderDetail = mongoDb.GetCollection<MongoOrderDetail>(MongoCollectionsName.OrderDetail);
        
    }
}