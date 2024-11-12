namespace EShop.Application.Contracts
{
    public interface ITagRepository:IGenericRepository<Tag>
    {
        Task<GetAllTagQueryResponse> GetAllAsync(SearchTagDto search);
    }
}
