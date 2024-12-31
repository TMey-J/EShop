using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Contracts.MongoDb
{
    public interface IMongoTagRepository:IMongoGenericRepository<MongoTag>
    {
        Task<GetAllTagsQueryResponse> GetAllAsync(SearchTagDto search);
    }
}
