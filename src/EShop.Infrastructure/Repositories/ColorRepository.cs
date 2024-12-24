using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories
{
    public class ColorRepository(SQLDbContext context) : GenericRepository<Color>(context), IColorRepository
    {
        private readonly DbSet<Color> _color = context.Set<Color>();

        public async Task CreateAllAsync(List<Color> colors)
        {
            await _color.AddRangeAsync(colors);
        }
    }
}