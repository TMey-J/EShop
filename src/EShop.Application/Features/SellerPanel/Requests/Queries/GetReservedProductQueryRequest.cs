using System.Text.Json.Serialization;

namespace EShop.Application.Features.SellerPanel.Requests.Queries;

public record GetReservedProductQueryRequest
    : IRequest<GetReservedProductQueryResponse>
{
    [DisplayName("شناسه محصول")]
    public long ProductId { get; set; }
    
    [DisplayName("شناسه رنگ")]
    public long ColorId { get; set; }
    
    [DisplayName("شناسه فروشنده")]
    [JsonIgnore]
    public long SellerId { get; set; }
}
public record GetReservedProductQueryResponse(ShowReservedProductDto Reserve);
