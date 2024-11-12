using EShop.Application.DTOs;
using EShop.Application.Features.AdminPanel.Requests.Queries.Tag;

namespace EShop.Application.Contracts
{
    public interface ITagRepository:IGenericRepository<Tag>
    {
        Task<GetAllTagQueryResponse> GetAllAsync(SearchTagDto search);
    }
}
