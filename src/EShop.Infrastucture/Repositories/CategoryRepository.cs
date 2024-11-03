using EShop.Application.Contracts;
using EShop.Domain.Entities;
using EShop.Infrastucture.Databases;
using System.Linq;

namespace EShop.Infrastucture.Repositories
{
    public class CategoryRepository(SQLDbContext context) : GenericRepository<Category>(context), ICategoryRepository
    {
        private readonly DbSet<Category> _category=context.Set<Category>();

        public async Task<HierarchyId?> GetLastChildHieaechyIdAsync(Category category)
        {
            return await _category.Where(x=>x.Parent.GetAncestor(1)==category.Parent).OrderByDescending(x=>x.Parent)
                .Select(x=>x.Parent).SingleOrDefaultAsync();
        }
    }
}
