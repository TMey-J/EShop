using System.Text.Json.Serialization;

namespace EShop.Application.Features.SellerPanel.Requests.Commands;

public record ReserveProductCommandRequest:IRequest<ReserveProductCommandResponse>
{
    public long ProductId { get; set; }
    [JsonIgnore]
    public long SellerId { get; set; }
    public string ColorCode { get; set; }
    public uint BasePrice { get; set; }
    public short Count { get; set; }
    public byte DiscountPercentage { get; set; }
    public DateTime? EndOfDiscount { get; set; }
}
public record ReserveProductCommandResponse;