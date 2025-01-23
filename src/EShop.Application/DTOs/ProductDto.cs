using MongoDB.Bson.Serialization.Attributes;

namespace EShop.Application.DTOs;

public record SearchProductDto : BaseSearchDto
{
    [DisplayName("مرتب کردن بر اساس")] public SortingProductBy SortingBy { get; set; }

    [DisplayName("عنوان فارسی")] public string Title { get; set; } = string.Empty;

    [DisplayName("عنوان انگلیسی")] public string EnglishTitle { get; set; } = string.Empty;

    [DisplayName("دسته بندی")] public string? CategoryTitle { get; set; }
}
public enum SortingProductBy
{
    Id,
    Title,
    EnglishTitle,
    Count
}
public record ShowAllProductForAdminPanelDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string EnglishTitle { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string CategoryTitle { get; set; } = string.Empty;
}

public record ShowProductForAdminPanelDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string EnglishTitle { get; set; } = string.Empty;
    public long CategoryId { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public List<string> Images { get; set; } = [];
    public List<ShowProductFeatureDto> Features { get; set; } = [];
}
public record ShowProductFeatureDto
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public bool IsCategoryFeature { get; set; }
}

public record ShowProductDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string EnglishTitle { get; set; } = string.Empty;
    public List<string> Categories { get; set; } = [];
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public List<string> Images { get; set; } = [];
    public Dictionary<string, string> Features { get; set; } = [];
    public Dictionary<string, string> Colors { get; set; } = [];
}