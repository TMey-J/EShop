namespace EShop.Application.DTOs;

public record SearchUserDto : BaseSearchDto
{
    [DisplayName("مرتب کردن بر اساس")]
    public SortingUserBy SortingBy { get; set; }
    
    [DisplayName("وضعیت فعالی")]
    public ActivationStatus ActivationStatus { get; set; }

    [DisplayName("نام کاربری")]
    public string UserName { get; init; } = string.Empty;
    
    [DisplayName("ایمیل")]
    public string Email { get; init; } = string.Empty;
    
    [DisplayName("شماره تلفن")]
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
