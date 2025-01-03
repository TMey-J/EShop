using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories
{
    public class ProductRepository(SQLDbContext context) : GenericRepository<Product>(context), IProductRepository
    {
        private readonly DbSet<Product> _product = context.Set<Product>();
        public async Task<List<ProductImages>> GetImagesByProductIdAsync(long productId)
        {
            var productImages = await _product.Include(x => x.Images.Where(i => i.ProductId == productId))
                .Select(x => x.Images).FirstAsync();
            return productImages.ToList();
        }
    }
}