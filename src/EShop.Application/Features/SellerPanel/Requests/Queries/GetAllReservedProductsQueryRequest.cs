using System.Text.Json.Serialization;

namespace EShop.Application.Features.SellerPanel.Requests.Queries;

public record GetAllReservedProductsQueryRequest
    : IRequest<GetAllReservedProductsQueryResponse>
{
    public SearchSellerProductDto Search { get; set; }
    [JsonIgnore]
    [DisplayName("شناسه فروشنده")]
    public long SellerId { get; set; }
}
public record GetAllReservedProductsQueryResponse(List<ShowReservedProductDto> ReservedProducts,SearchSellerProductDto Search,int PageCount);
