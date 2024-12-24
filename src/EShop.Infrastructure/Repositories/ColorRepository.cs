using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories
{
    public class ColorRepository(SQLDbContext context) : GenericRepository<Color>(context), IColorRepository
    {
        private readonly DbSet<Color> _color = context.Set<Color>();
        
    }
}