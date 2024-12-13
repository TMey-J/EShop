namespace EShop.Application.DTOs;

public record SearchFeatureDto : BaseSearchDto
{
    public SortingFeatureBy SortingBy { get; set; }
    
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