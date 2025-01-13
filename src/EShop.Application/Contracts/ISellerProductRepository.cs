namespace EShop.Application.Contracts
{
    public interface ISellerProductRepository
    {
        Task ReserveProductAsync(SellerProduct sellerProduct);
        Task<bool> IsExistAsync(long sellerId, long productId, long colorId);
        Task SaveChangesAsync();
    }
}