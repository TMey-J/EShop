namespace EShop.Application.Features.AdminPanel.Category.Requests.Queries;

public record GetAllCategoryQueryRequest(SearchCategoryDto? Search):IRequest<GetAllCategoryQueryResponse>;
public record GetAllCategoryQueryResponse(List<ShowCategoryDto> Categories,SearchCategoryDto? Search,int? PageCount);