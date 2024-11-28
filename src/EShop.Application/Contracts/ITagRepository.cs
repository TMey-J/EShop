namespace EShop.Application.Contracts
{
    public interface ITagRepository:IGenericRepository<Tag>
    {
        Task<GetAllTagsQueryResponse> GetAllAsync(SearchTagDto search);
    }
}
