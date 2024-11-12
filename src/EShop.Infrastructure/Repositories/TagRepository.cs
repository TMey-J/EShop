using EShop.Infrastructure.Databases;
using EShop.Application.Features.AdminPanel.Requests.Queries.Tag;

namespace EShop.Infrastructure.Repositories
{
    public class TagRepository(SQLDbContext context) : GenericRepository<Tag>(context), ITagRepository
    {
        private readonly DbSet<Tag> _tag = context.Set<Tag>();
        public async Task<GetAllTagQueryResponse> GetAllAsync(SearchTagDto search)
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

            var tags = await tagQuery.Select
                (x => new ShowTagDto(x.Id, x.Title)).ToListAsync();

            return new GetAllTagQueryResponse(tags, search, pagination.pageCount);
        }
    }
}