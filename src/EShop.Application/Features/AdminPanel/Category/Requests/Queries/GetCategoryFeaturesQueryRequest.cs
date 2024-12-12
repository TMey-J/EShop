namespace EShop.Application.Features.AdminPanel.Category.Requests.Queries;

public record GetCategoryFeaturesQueryRequest:IRequest<GetCategoryFeaturesQueryResponse>
{
    [DisplayName("شناسه دسته بندی")]
    public long CategoryId { get; set; }
}
public record GetCategoryFeaturesQueryResponse(Dictionary<long, string> Features);