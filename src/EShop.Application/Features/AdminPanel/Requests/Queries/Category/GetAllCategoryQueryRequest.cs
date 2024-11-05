namespace EShop.Application.Features.AdminPanel.Requests.Queries.Category;

public record GetAllCategoryQueryRequest:IRequest<List<GetAllCategoryQueryResponse>>;
public record GetAllCategoryQueryResponse(long Id,string Title,long? ParentId,string? PictureName);