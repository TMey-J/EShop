using System.Text.Json.Serialization;

namespace EShop.Application.DTOs;

public record SearchSellerProductDto : BaseSearchDto
{
    [DisplayName("مرتب کردن بر اساس")]
    public SortingSellerProductBy SortingBy { get; set; }
    
    [DisplayName("عنوان محصول")]
    public string Title { get; init; } = string.Empty;

    public long CategoryId { get; init; }
    [JsonIgnore]
    public override DeleteStatus DeleteStatus { get; set; }
}

public record ShowAllReservedProductDto
{
    public long ProductId { get; set; }
    public long ColorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public short Count { get; set; }
    public uint BasePrice { get; set; }
    public string ColorCode { get; set; }=string.Empty;
}
public record ShowReservedProductDto: ShowAllReservedProductDto
{
    public byte DiscountPercentage { get; set; }
    public DateTime? EndOfDiscount { get; set; }
}
public record ShowProductForSellerPanelDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
}
public record GetSellersProductDto
{
    public long SellerId { get; set; }
    public string ShopName { get; set; }=string.Empty;
    public long ColorId { get; set; }
    public short Count { get; set; }
    public uint BasePrice { get; set; }
    public uint PriceWithDiscount { get; set; }
    public byte DiscountPercentage { get; set; }
    public DateTime? EndOfDiscount { get; set; }
}
public enum SortingSellerProductBy
{
    ProductId,
    Product_Title,
    Count,
    BasePrice
}