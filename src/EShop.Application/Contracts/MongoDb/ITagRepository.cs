namespace EShop.Application.Contracts.MongoDb
{
    public interface IMongoTagRepository:IMongoGenericRepository<Tag>
    {
        Task<GetAllTagsQueryResponse> GetAllAsync(SearchTagDto search);
    }
}
