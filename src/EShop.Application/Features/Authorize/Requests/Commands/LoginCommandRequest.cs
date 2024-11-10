using System.ComponentModel;
using static EShop.Application.Features.Authorize.Requests.Commands.LoginCommandRequest;

namespace EShop.Application.Features.Authorize.Requests.Commands;

public record LoginCommandRequest() : IRequest<LoginCommandResponde>
{
    [DisplayName("ایمیل/شماره تلفن")]
    public string EmailOrPhoneNumber { get; set; } = string.Empty;

    [DisplayName("کلمه عبور")]
    public string Password { get; set; } = string.Empty;
}
public record LoginCommandResponde(string token);
