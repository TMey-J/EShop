namespace EShop.Application.Features.AdminPanel.Tag.Requests.Queries;

public record GetTagQueryRequest() : IRequest<GetTagQueryResponse>
{
    [DisplayName("شناسه")]
    public long Id { get; init; }
}
public record GetTagQueryResponse(long Id,string Title,bool IsConfirmed);