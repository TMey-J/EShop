namespace EShop.Application.DTOs;

public record SearchUserDto : BaseSearchDto
{
    public SortingUserBy SortingBy { get; set; }
    public ActivationStatus ActivationStatus { get; set; }

    public string UserName { get; init; } = string.Empty;
    
    public string Email { get; init; } = string.Empty;
    
    public string PhoneNumber { get; init; } = string.Empty;
}

public record ShowUserDto(
    long Id,
    string UserName,
    string? EmailOrPhoneNumber,
    bool IsActive,
    string Avatar);

public enum SortingUserBy
{
    Id,
    UserName,
    Email,
    PhoneNumber,
    IsActive,
    Avatar
}
public enum ActivationStatus
{
    True,
    False,
    OnlyActive
}
