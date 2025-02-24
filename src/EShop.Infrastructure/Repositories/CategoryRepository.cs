﻿using EShop.Application.Features.AdminPanel.Category.Requests.Queries;
using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories
{
    public class CategoryRepository(SQLDbContext context) : GenericRepository<Category>(context), ICategoryRepository
    {
        private readonly DbSet<Category> _category = context.Set<Category>();
        private readonly DbSet<CategoryFeature> _categoryFeatures = context.Set<CategoryFeature>();

        public async Task<bool> IsHasChild(Category category)
        {
            return await _category.AnyAsync(x => x.ParentId == category.Id);
        }
        public async Task<GetAllCategoryQueryResponse> GetAllAsync(SearchCategoryDto search)
        {
            var category = _category.AsQueryable().IgnoreQueryFilters();

            #region Search

            category = category.CreateContainsExpression(nameof(Category.Title), search.Title);

            #endregion

            #region Sort

            category = category.CreateOrderByExpression(search.SortingBy.ToString(), search.SortingAs);

            category = category.CreateDeleteStatusExpression(nameof(BaseEntity.IsDelete), search.DeleteStatus);

            #endregion

            #region Paging

            (IQueryable<Category> query, int pageCount) pagination =
                category.Page(search.Pagination.CurrentPage, search.Pagination.TakeRecord);
            category = pagination.query;

            #endregion

            var categories = await category.Select
            (x => new ShowCategoryDto(x.Id, x.Title,
                x.ParentId,
                x.Picture)).ToListAsync();

            return new GetAllCategoryQueryResponse(categories, search, pagination.pageCount);
        }

        public async Task<Category?> FindByIdWithIncludeFeatures(long categoryId)
        {
            return await _category.Include(x=>x.CategoryFeatures)!.
                ThenInclude(x=>x.Feature).
                SingleOrDefaultAsync(x=>x.Id == categoryId); 
        }
    }
}