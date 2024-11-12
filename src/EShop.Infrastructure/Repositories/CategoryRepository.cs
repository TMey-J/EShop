using EShop.Application.Features.AdminPanel.Category.Requests.Queries;
using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories
{
    public class CategoryRepository(SQLDbContext context) : GenericRepository<Category>(context), ICategoryRepository
    {
        private readonly DbSet<Category> _category = context.Set<Category>();

        public async Task<HierarchyId?> GetLastChildHierarchyIdAsync(Category category)
        {
            return await _category.Where(x => x.Parent.GetAncestor(1) == category.Parent)
                .OrderByDescending(x => x.Parent)
                .Select(x => x.Parent).FirstOrDefaultAsync();
        }

        public async Task<long?> GetParentIdWithHierarchyIdAsync(HierarchyId categoryHierarchyId)
        {
            var parentHierarchyId = categoryHierarchyId.GetAncestor(1);
            var parentId = await _category.Where(x => x.Parent == parentHierarchyId)
                .Select(x => x.Id).SingleOrDefaultAsync();
            return parentId != 0 ? parentId : null;
        }

        public async Task<List<Category>> GetCategoryChildrenAsync(Category category)
        {
            return await _category.Where(x =>
                x.Id != category.Id && x.Parent.IsDescendantOf(category.Parent)).ToListAsync();
        }

        public async Task<GetAllCategoryQueryResponse> GetAllAsync(SearchCategoryDto search)
        {
            var category = _category.AsQueryable().IgnoreQueryFilters();

            #region Search

            category = category.CreateContainsExpression(nameof(Category.Title), search.Title);

            #endregion

            #region Sort

            category = category.CreateOrderByExperssion(search.SortingBy.ToString(), search.SortingAs);

            category = category.CreateDeleteStatusExperssion(nameof(BaseEntity.IsDelete), search.DeleteStatus);

            #endregion

            #region Paging

            (IQueryable<Category> query, int pageCount) pagination =
                category.Page(search.Pagination.CurrentPage, search.Pagination.TakeRecord);
            category = pagination.query;

            #endregion

            var categories = await category.Select
            (x => new ShowCategoryDto(x.Id, x.Title,
                _category.SingleOrDefault(c => c.Parent == x.Parent.GetAncestor(1))!.Id,
                x.Picture)).ToListAsync();

            return new GetAllCategoryQueryResponse(categories, search, pagination.pageCount);
        }
    }
}