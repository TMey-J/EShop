using EShop.Application.Features.Comment.Requests.Queries;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Contracts.MongoDb
{
    public interface IMongoCommentRepository:IMongoGenericRepository<MongoComment>
    {
        Task<GetAllCommentsQueryResponse> GetAllForProductAsync(long productId,Pagination pagination,CancellationToken cancellationToken);
    }
}
