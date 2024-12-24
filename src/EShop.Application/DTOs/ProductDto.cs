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
    public uint BasePrice { get; set; }

    [DisplayName("تعداد")]
    public int Count { get; set; }
}

public record ShowProductDto(
    long Id,
    string Title,
    string EnglishTitle,
    uint BasePrice,
    byte DiscountPercentage,
    string Image,
    int Count,
    string Category);
public enum SortingProductBy
{
    Id,
    Title,
    EnglishTitle,
    BasePrice,
    DiscountPercentage,
    Count
}

public class ReadProduct : BaseEntity
{
    public string Title { get; set; }=string.Empty;
    public string EnglishTitle { get; set; }=string.Empty;
    public uint BasePrice { get; set; }
    public byte? DiscountPercentage { get; set; }
    public int Count { get; set; }
    public long SellerId { get; set; }
    public string CategoryTitle { get; set; } = string.Empty;
    public string Description { get; set; }=string.Empty;
    public DateTime? EndOfDiscount { get; set; }
    public Dictionary<string, string> Colors { get; set; } = [];
    public List<string> Tags { get; set; }= [];
    public List<string> Images { get; set; }= [];
    
};