namespace EShop.Domain.Entities;

public class SellerProduct
{
    public long SellerId { get; set; }
    public long ProductId { get; set; }
    
    public int Count { get; set; }

    #region Relations
    public Seller? Seller { get; set; }
    public Product? Product { get; set; }
    #endregion
}