namespace EShop.Application.Features.AdminPanel.User.Requests.Commands;

public record UpdateUserCommandRequest : IRequest<UpdateUserCommandResponse>
{
    [DisplayName("شناسه")]
    public long Id { get; set; }
    
    [DisplayName("نام کاربری")]
    public string UserName { get; set; } = string.Empty;

    [DisplayName("ایمیل / شماره تلفن")]
    public string EmailOrPhoneNumber { get; set; } = string.Empty;

    [DisplayName("کلمه عبور")]
    public string? Password { get; set; } = string.Empty;

    [DisplayName("آواتار")]
    public string? NewAvatar { get; set; }
    
    public bool IsActive { get; set; }
    
    [DisplayName("نقش های کاربر")]
    public List<string> Roles { get; set; } = [];
}
public record UpdateUserCommandResponse;