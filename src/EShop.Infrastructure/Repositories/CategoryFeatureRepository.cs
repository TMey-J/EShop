using EShop.Infrastructure.Databases;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;

namespace EShop.Infrastructure.Repositories
{
    public class CategoryFeatureRepository(SQLDbContext context) : ICategoryFeatureRepository
    {
        private readonly SQLDbContext _context=context;
        private readonly DbSet<CategoryFeature> _categoryFeature = context.Set<CategoryFeature>();

        public void DeleteAllFeaturesFromCategory(long categoryId)
        { 
            _categoryFeature.RemoveRange(_categoryFeature.Where(c => c.CategoryId == categoryId));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}