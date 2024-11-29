using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Tag = EShop.Domain.Entities.Tag;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoTagRepository(MongoDbContext mongoDb) : MongoGenericRepository<Tag>(mongoDb), IMongoTagRepository
    {
        private readonly IMongoCollection<Tag> _tag = mongoDb.GetCollection<Tag>();

        public async Task<GetAllTagsQueryResponse> GetAllAsync(SearchTagDto search)
        {
            var tagQuery = _tag.AsQueryable().IgnoreQueryFilters();

            #region Search

            tagQuery = tagQuery.CreateContainsExpression(nameof(Tag.Title), search.Title);

            #endregion

            #region Sort

            tagQuery = tagQuery.CreateOrderByExperssion(search.SortingBy.ToString(), search.SortingAs);

            tagQuery = tagQuery.CreateDeleteStatusExperssion(nameof(BaseEntity.IsDelete), search.DeleteStatus);

            #endregion

            #region Paging

            (IQueryable<Tag> query, int pageCount) pagination =
                tagQuery.Page(search.Pagination.CurrentPage, search.Pagination.TakeRecord);
            tagQuery = pagination.query;

            #endregion

            var tags = await MongoQueryable.ToListAsync(tagQuery.Select
                (x => new ShowTagDto(x.Id, x.Title)));

            return new GetAllTagsQueryResponse(tags, search, pagination.pageCount);
        }
    }
}