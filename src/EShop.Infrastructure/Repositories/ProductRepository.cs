using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories
{
    public class ProductRepository(SQLDbContext context) : GenericRepository<Product>(context), IProductRepository
    {
        private readonly DbSet<Product> _product = context.Set<Product>();
        
    }
}