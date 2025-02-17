namespace EShop.Application.DTOs;

public record SearchFeatureDto : BaseSearchDto
{
    [DisplayName("مرتب کردن بر اساس")]
    public SortingFeatureBy SortingBy { get; set; }
    
    [DisplayName("نام ویژگی")]
    public string Name { get; init; }=string.Empty;
}

public record ShowFeatureDto(
    long Id,
    string Name);
public enum SortingFeatureBy
{
    Id,
    Title
}