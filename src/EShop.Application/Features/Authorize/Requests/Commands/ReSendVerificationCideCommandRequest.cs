using System.ComponentModel;

namespace EShop.Application.Features.Authorize.Requests.Commands
{
    public record ReSendVerificationCideCommandRequest:IRequest<ReSendVerificationCideCommandRespose>
    {
        [DisplayName("ایمیل/تلفن همراه")]
        public string EmailOrPhoneNumber { get; set; } = string.Empty;
    }
    public record ReSendVerificationCideCommandRespose(string PhoneNumber, int? ResendCodeSeconds, DateTime? SendCodeLastTime);
}
