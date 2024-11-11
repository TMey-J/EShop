namespace EShop.Application.DTOs;

public abstract record SearchCategoryDto : BaseSearchDTO
{
    public SortingBy SortingBy { get; set; }
    public string Title { get; init; }=string.Empty;
}

public record ShowCategoryDto(
    long Id,
    string Title,
    long? ParentId,
    string? PictureName);
public enum SortingBy
{
    Id,
    Title
}