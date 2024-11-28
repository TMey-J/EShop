namespace EShop.Application.DTOs;

public record SearchCategoryDto : BaseSearchDto
{
    [DisplayName("مرتب کردن بر اساس")]
    public SortingCategoryBy SortingBy { get; set; }
    
    [DisplayName("عنوان")]
    public string Title { get; init; }=string.Empty;
}

public record ShowCategoryDto(
    long Id,
    string Title,
    long? ParentId,
    string? PictureName);
public enum SortingCategoryBy
{
    Id,
    Title
}