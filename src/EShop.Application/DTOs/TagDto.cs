namespace EShop.Application.DTOs;

public record SearchTagDto : BaseSearchDto
{
    [DisplayName("مرتب کردن بر اساس")]
    public SortingTagBy SortingBy { get; set; }
    
    [DisplayName("عنوان")]
    public string Title { get; init; }=string.Empty;
}

public record ShowTagDto(
    long Id,
    string Title);
public enum SortingTagBy
{
    Id,
    Title,
    IsConfirmed
}