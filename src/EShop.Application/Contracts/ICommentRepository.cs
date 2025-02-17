using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Contracts
{
    public interface ICommentRepository:IGenericRepository<Comment>
    {
        Task<bool> IsReplyValid(long productId, long commentId);
    }
}
