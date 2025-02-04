using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Contracts.MongoDb
{
    public interface IMongoOrderRepository:IMongoGenericRepository<MongoOrder>
    {
    }
}
