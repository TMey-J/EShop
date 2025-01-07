using MongoDB.Bson.Serialization.Attributes;

namespace EShop.Application.DTOs;

public record SearchProductDto : BaseSearchDto
{
    [DisplayName("مرتب کردن بر اساس")]
    public SortingProductBy SortingBy { get; set; }
    
    [DisplayName("عنوان فارسی")]
    public string Title { get; set; } = string.Empty;

    [DisplayName("عنوان انگلیسی")]
    public string EnglishTitle { get; set; } = string.Empty;

    [DisplayName("قیمت")]
    public uint? BasePrice { get; set; }

    [DisplayName("دسته بندی")]
    public string? CategoryTitle { get; set; }
}

public record ShowAllProductDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string EnglishTitle { get; set; } = string.Empty;
    public uint BasePrice { get; set; }
    public byte DiscountPercentage { get; set; }
    public uint PriceWithDiscount { get; set; }
    public string Image { get; set; } = string.Empty;
    public int Count { get; set; }
    public string CategoryTitle { get; set; } = string.Empty;
}

public record ShowProductDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string EnglishTitle { get; set; } = string.Empty;
    public uint BasePrice { get; set; }
    public byte DiscountPercentage { get; set; }
    public uint? PriceWithDiscount { get; set; }
    public int Count { get; set; }
    public long CategoryId { get; set; }
    public DateTime? EndOfDiscount { get; set; }
    public List<string> ColorsCode { get; set; } = [];
    public List<string> Tags { get; set; } = [];
    public List<string> Images { get; set; } = [];
}
public enum SortingProductBy
{
    Id,
    Title,
    EnglishTitle,
    BasePrice,
    DiscountPercentage,
    Count
}