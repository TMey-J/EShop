using System.ComponentModel;
using System.Text.Json.Serialization;

namespace EShop.Application.Features.Authorize.Requests.Commands;

public record VerifyPhoneNumberCommandRequest : IRequest<VerifyPhoneNumberCommandResponse>
{
    [JsonIgnore]
    [DisplayName("تلفن همراه")]
    public string PhoneNumber { get; set; } = string.Empty;

    [DisplayName("کد فعال سازی")]
    public string Code { get; set; } = string.Empty;

}
public record VerifyPhoneNumberCommandResponse();
