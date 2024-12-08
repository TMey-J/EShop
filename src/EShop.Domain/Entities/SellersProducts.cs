using EShop.Domain.Entities;

namespace EShop.Domain;

public class SellerProduct
{
    public long SellerId { get; set; }
    public long ProductId { get; set; }
    public int Count { get; set; }

    #region Relations

    public SellerBase? Seller { get; set; }
    public Product? Product { get; set; }
    #endregion
}