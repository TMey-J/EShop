namespace EShop.Application.Features.AdminPanel.Feature.Requests.Queries;

public record GetFeatureQueryRequest: IRequest<GetFeatureQueryResponse>
{
    [DisplayName("شناسه")]
    public long Id { get; init; }
}
public record GetFeatureQueryResponse(long Id,string Name);