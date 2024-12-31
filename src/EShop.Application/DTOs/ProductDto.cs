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

public record ShowProductDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string EnglishTitle { get; set; } = string.Empty;
    public uint BasePrice { get; set; }
    public uint? DiscountPercentage { get; set; }
    public uint? PriceWithDiscount { get; set; }
    public string Image { get; set; } = string.Empty;
    public int Count { get; set; }
    public string CategoryTitle { get; set; } = string.Empty;
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