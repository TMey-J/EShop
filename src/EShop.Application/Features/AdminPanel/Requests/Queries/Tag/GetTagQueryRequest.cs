using EShop.Application.DTOs;

namespace EShop.Application.Features.AdminPanel.Requests.Queries.Tag;

public record GetTagQueryRequest() : IRequest<GetTagQueryResponse>
{
    [DisplayName("شناسه")]
    public long Id { get; init; }
}
public record GetTagQueryResponse(long Id,string Title);