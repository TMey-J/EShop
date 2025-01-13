namespace EShop.Domain.Entities.Mongodb;

public class MongoSellerProduct
{
    public long Id { get; set; }
    public long SellerId { get; set; }
    public long ProductId { get; set; }
    public long ColorId { get; set; }
    public short Count { get; set; }
    public uint BasePrice { get; set; }
    public byte DiscountPercentage { get; set; }
    public DateTime? EndOfDiscount { get; set; }
};