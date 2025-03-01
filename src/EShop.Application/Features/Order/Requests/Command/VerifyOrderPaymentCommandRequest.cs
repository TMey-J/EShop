using System.Text.Json.Serialization;

namespace EShop.Application.Features.Order.Requests.Command;

public record VerifyOrderPaymentCommandRequest : IRequest<VerifyOrderPaymentCommandResponse>
{
    [DisplayName("شناسه سفارش")]
    public long OrderId { get; set; }
    [DisplayName("شناسه پرداخت")]
    public string Authority { get; set; } = null!;
}

public record VerifyOrderPaymentCommandResponse(bool IsSuccess, int? RefId);