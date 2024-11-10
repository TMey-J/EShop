using Microsoft.EntityFrameworkCore;

namespace EShop.Application.DTOs.Category;

public record SearchCategoryDTO : BaseSearchDTO
{
    public SortingBy SortingBy { get; set; }
    public string Title { get; init; }=string.Empty;
}

public record ShowCategoryDTO(
    long Id,
    string Title,
    long? ParentId,
    string? PictureName);
public enum SortingBy
{
    Id,
    Title
}