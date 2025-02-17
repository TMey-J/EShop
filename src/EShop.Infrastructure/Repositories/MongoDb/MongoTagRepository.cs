using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Tag = EShop.Domain.Entities.Tag;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoTagRepository(MongoDbContext mongoDb) : MongoGenericRepository<MongoTag>(mongoDb,MongoCollectionsName.Tag), IMongoTagRepository
    {
        private readonly IMongoCollection<MongoTag> _tag = mongoDb.GetCollection<MongoTag>(MongoCollectionsName.Tag);

        public async Task<GetAllTagsQueryResponse> GetAllAsync(SearchTagDto search)
        {
            var tagQuery = _tag.AsQueryable().IgnoreQueryFilters();

            #region Search

            tagQuery = tagQuery.CreateContainsExpression(nameof(Tag.Title), search.Title);

            #endregion

            #region Sort

            tagQuery = tagQuery.CreateOrderByExpression(search.SortingBy.ToString(), search.SortingAs);

            tagQuery = tagQuery.CreateDeleteStatusExpression(nameof(BaseEntity.IsDelete), search.DeleteStatus);

            #endregion

            #region Paging

            (IQueryable<MongoTag> query, int pageCount) pagination =
                tagQuery.Page(search.Pagination.CurrentPage, search.Pagination.TakeRecord);
            tagQuery = pagination.query;

            #endregion

            var tags = await MongoQueryable.ToListAsync(tagQuery.Select
                (x => new ShowTagDto(x.Id, x.Title)));

            return new GetAllTagsQueryResponse(tags, search, pagination.pageCount);
        }
    }
}