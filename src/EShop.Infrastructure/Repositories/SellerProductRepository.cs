using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories;

public class SellerProductRepository(SQLDbContext context):ISellerProductRepository
{
    private readonly DbSet<SellerProduct>_sellerProduct=context.Set<SellerProduct>();
    private readonly SQLDbContext _context = context;
    public async Task ReserveProductAsync(SellerProduct sellerProduct)
    {
        await _sellerProduct.AddAsync(sellerProduct);
    }

    public void UpdateReserveProductAsync(SellerProduct sellerProduct)
    {
        _sellerProduct.Update(sellerProduct);
    }

    public async Task<bool> IsExistAsync(long sellerId, long productId, long colorId)
    {
        return await _sellerProduct.AnyAsync(x=>x.SellerId == sellerId && x.ProductId == productId && x.ColorId==colorId);
    }

    public async Task<SellerProduct?> FindReserveAsync(long sellerId, long productId, long colorId)
    {
        return await _sellerProduct.Include(x=>x.Color).
            Include(x=>x.Product).ThenInclude(x=>x.Images)
            .SingleOrDefaultAsync(x=>x.ProductId == productId && x.ColorId == colorId);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}