using System.ComponentModel;

namespace EShop.Application.Features.Authorize.Requests.Commands
{
    public record ReSendSmsCommandRequest:IRequest<ReSendSmsCommandResponse>
    {
        [DisplayName("تلفن همراه")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
    public record ReSendSmsCommandResponse(string PhoneNumber, int? ResendCodeSeconds, DateTime? SendCodeLastTime);
}
