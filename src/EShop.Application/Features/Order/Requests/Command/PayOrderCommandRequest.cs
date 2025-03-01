using System.Text.Json.Serialization;

namespace EShop.Application.Features.Order.Requests.Command;

public record PayOrderCommandRequest : IRequest<PayOrderCommandResponse>
{
    [DisplayName("شناسه کاربر")]
    [JsonIgnore]
    public long UserId { get; set; }

    [DisplayName("شناسه سفارش")]
    public long OrderId { get; set; }
    [DisplayName("آدرس برگشت")]
    public string CallbackUrl { get; set; } = null!;
}

public record PayOrderCommandResponse(string Authority, string GetWayUrl);