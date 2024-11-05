using EShop.Application.Contracts;
using EShop.Domain.Entities;
using System.Linq;
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
                .Select(x => x.Parent).SingleOrDefaultAsync();
        }

        public async Task<long?> GetParentIdWithHierarchyIdAsync(HierarchyId categoryHierarchyId)
        {
            var parentHierarchyId = categoryHierarchyId.GetAncestor(1);
            var parentId = await _category.Where(x => x.Parent == parentHierarchyId)
                .Select(x => x.Id).SingleOrDefaultAsync();
            return parentId != 0 ? parentId : null;
            ;
        }
    }
}