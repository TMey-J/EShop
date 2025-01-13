using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities;

public class SellerProduct
{
    public long SellerId { get; set; }
    public long ProductId { get; set; }
    public long ColorId { get; set; }
    [Required]
    public short Count { get; set; }
    [Required]
    public uint BasePrice { get; set; }
    [Length(1,100)]
    public byte DiscountPercentage { get; set; }
    
    public DateTime? EndOfDiscount { get; set; }

    #region Relations
    public Seller? Seller { get; set; }
    public Product? Product { get; set; }
    public Color Color { get; set; }
    #endregion
}