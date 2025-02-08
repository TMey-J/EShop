using System.Text.Json.Serialization;

namespace EShop.Application.Features.Order.Requests.Queries;

public record GetAllOrdersQueryRequest : IRequest<GetAllOrdersQueryResponse>
{
    [JsonIgnore]
    [DisplayName("شناسه کاربر")]
    public long UserId { get; set; }
}

public record GetAllOrdersQueryResponse(List<ShowOrderDetailsDto> Orders,uint TotalSum);