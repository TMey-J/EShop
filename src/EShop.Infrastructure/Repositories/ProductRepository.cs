using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories
{
    public class ProductRepository(SQLDbContext context) : GenericRepository<Product>(context), IProductRepository
    {
        private readonly DbSet<Product> _product = context.Set<Product>();
        private readonly DbSet<SellerProduct> _sellerProduct = context.Set<SellerProduct>();
        private readonly DbSet<ProductTag> _productTags = context.Set<ProductTag>();
        private readonly DbSet<ProductFeature> _productFeatures = context.Set<ProductFeature>();

        public async Task<List<ProductImages>> GetImagesByProductIdAsync(long productId)
        {
            var productImages = await _product.Include(x => x.Images.Where(i => i.ProductId == productId))
                .Select(x => x.Images).FirstAsync();
            return productImages.ToList();
        }

        public async Task<List<Color>> GetProductColorsAsync(long productId)
        {
            return await _sellerProduct.Include(x => x.Color)
                .Where(x => x.ProductId == productId).Select(x => x.Color)
                .ToListAsync();
        }

        public async Task<List<Tag>> GetProductTagsAsync(long productId)
        {
            return await _product.Include(x => x.ProductTags.Where(p => p.ProductId == productId))
                .ThenInclude(x => x.Tag)
                .SelectMany(x => x.ProductTags)
                .Select(x => x.Tag)
                .ToListAsync();
        }

        public async Task<List<ProductFeature>> GetProductFeaturesAsync(long productId)
        {
            return await _productFeatures.Where(x=>x.ProductId == productId)
                .ToListAsync();
        }

        public async Task DeleteTagsAsync(long productId)
        {
            await _productTags.Where(x => x.ProductId == productId)
                .ExecuteDeleteAsync();
        }

        public async Task DeleteFeaturesAsync(long productId)
        {
            await _productFeatures.Where(x => x.ProductId == productId)
                .ExecuteDeleteAsync();
        }
    }
}