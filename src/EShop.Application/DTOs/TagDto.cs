namespace EShop.Application.DTOs;

public record SearchTagDto : BaseSearchDto
{
    public SortingTagBy SortingBy { get; set; }
    
    public string Title { get; init; }=string.Empty;
}

public record ShowTagDto(
    long Id,
    string Title);
public enum SortingTagBy
{
    Id,
    Title
}