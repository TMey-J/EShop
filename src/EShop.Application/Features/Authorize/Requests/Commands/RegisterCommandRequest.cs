using MediatR;
using System.ComponentModel;

namespace EShop.Application.Features.Authorize.Requests.Commands;

public record RegisterCommandRequest():IRequest<RegisterCommandResponse>
{
    [DisplayName("نام کاربری")]
    public string UserName { get; set; } = string.Empty;

    [DisplayName("ایمیل / شماره تلفن")]
    public string EmailOrPhoneNumber { get; set; } = string.Empty;

    [DisplayName("کلمه عبور")]
    public string Password { get; set; } = string.Empty;

    [DisplayName("تکرار کلمه عبور")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
public record RegisterCommandResponse(string EmailOrPhoneNumber,int? ResendCodeSeconds,DateTime? SendCodeLastTime);
