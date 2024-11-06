namespace EShop.Application.Features.AdminPanel.Requests.Queries.Category;

public record GetCategoryQueryRequest:IRequest<GetCategoryQueryResponse>
{
    [DisplayName("شناسه")]
    public long Id { get; set; }
}
public record GetCategoryQueryResponse(long Id,string Title,long? ParentId);