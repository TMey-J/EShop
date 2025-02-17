using System.Text.Json.Serialization;

namespace EShop.Application.Features.Order.Requests.Command;

public record ChangeOrderCountCommandRequest : IRequest<ChangeOrderCountCommandResponse>
{
    [JsonIgnore]
    [DisplayName("شناسه کاربر")]
    public long UserId { get; set; }
    [DisplayName("شناسه سفارش")]
    public long OrderDetailId { get; set; }
    [DisplayName("تعداد")]
    public short Quantity { get; set; }
}

public record ChangeOrderCountCommandResponse;