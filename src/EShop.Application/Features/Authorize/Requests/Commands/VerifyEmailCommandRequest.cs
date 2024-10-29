using System.ComponentModel;

namespace EShop.Application.Features.Authorize.Requests.Commands
{
    public record VerifyEmailCommandRequest : IRequest<VerifyEmailCommandResponse>
    {
        [DisplayName("ایمیل")]
        public string Email { get; set; } = string.Empty;

        [DisplayName("کد فعال ساز")]
        public string Token { get; set; } = string.Empty;

    }
    public record VerifyEmailCommandResponse();
}
