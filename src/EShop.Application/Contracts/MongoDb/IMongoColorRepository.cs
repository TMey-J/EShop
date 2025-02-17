using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Contracts.MongoDb
{
    public interface IMongoColorRepository:IMongoGenericRepository<MongoColor>
    {
        Task<List<MongoColor>> GetAllColorsByIdAsync(List<long> ids);
    }
}
