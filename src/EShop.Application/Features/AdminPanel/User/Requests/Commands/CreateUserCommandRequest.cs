namespace EShop.Application.Features.AdminPanel.User.Requests.Commands;

public record CreateUserCommandRequest : IRequest<CreateUserCommandResponse>
{
    [DisplayName("نام کاربری")]
    public string UserName { get; set; } = string.Empty;

    [DisplayName("ایمیل / شماره تلفن")]
    public string EmailOrPhoneNumber { get; set; } = string.Empty;

    [DisplayName("کلمه عبور")]
    public string Password { get; set; } = string.Empty;

    [DisplayName("تکرار کلمه عبور")]
    public string ConfirmPassword { get; set; } = string.Empty;
    
    [DisplayName("آواتار")]
    public string? Avatar { get; set; } = string.Empty;
}
public record CreateUserCommandResponse;