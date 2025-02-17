using EShop.Application.Features.AdminPanel.Category.Requests.Queries;
using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories
{
    public class CommentRepository(SQLDbContext context) : GenericRepository<Comment>(context), ICommentRepository
    {
        private readonly DbSet<Comment> _comment = context.Set<Comment>();

        public async Task<bool> IsReplyValid(long productId, long commentId)
        {
            return await _comment.AnyAsync(x=>x.ProductId==productId && x.Id==commentId);
        }
    }
}