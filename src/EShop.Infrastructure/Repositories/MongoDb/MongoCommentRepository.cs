using Blogger.Application.Common.Exceptions;
using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Category.Requests.Queries;
using EShop.Application.Features.Comment.Requests.Queries;
using EShop.Application.Model;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoCommentRepository(MongoDbContext mongoDb)
        : MongoGenericRepository<MongoComment>(mongoDb,MongoCollectionsName.Comment),IMongoCommentRepository
    {
        private readonly IMongoCollection<MongoComment> _comment = mongoDb.GetCollection<MongoComment>(MongoCollectionsName.Comment);

        public async Task<GetAllCommentsQueryResponse> GetAllForProductAsync(long productId,Pagination page,CancellationToken cancellationToken)
        {
            var commentQuery=_comment.AsQueryable().Where(x=>x.ProductId==productId);
            
            #region Paging

            (IQueryable<MongoComment> query, int pageCount) pagination =
                commentQuery.Page(page.CurrentPage, page.TakeRecord);
            commentQuery = pagination.query;

            #endregion

            var comments= await MongoQueryable.ToListAsync(commentQuery.Select(x =>
                new ShowCommentDto(x.Id, x.Body, x.ParentId, x.Rating, x.CreateDateTime, x.ModifiedDateTime)
            ), cancellationToken);
            return new GetAllCommentsQueryResponse(comments, pagination.pageCount);
        }
    }
}