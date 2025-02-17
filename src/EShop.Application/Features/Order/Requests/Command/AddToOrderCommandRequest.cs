using System.Text.Json.Serialization;

namespace EShop.Application.Features.Order.Requests.Command;

public record AddToOrderCommandRequest : IRequest<AddToOrderCommandResponse>
{
    [JsonIgnore]
    [DisplayName("شناسه کاربر")]
    public long UserId { get; set; }
    [DisplayName("شناسه محصول")]
    public long ProductId { get; set; }
    [DisplayName("شناسه فروشنده")]
    public long SellerId { get; set; }
    [DisplayName("شناسه شناسه رنگ")]
    public long ColorId { get; set; }
    [DisplayName("تعداد")]
    public short Quantity { get; set; }
}

public record AddToOrderCommandResponse;