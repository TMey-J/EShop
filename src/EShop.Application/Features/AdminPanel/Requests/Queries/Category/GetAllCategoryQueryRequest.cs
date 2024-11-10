using EShop.Application.DTOs.Category;

namespace EShop.Application.Features.AdminPanel.Requests.Queries.Category;

public record GetAllCategoryQueryRequest(SearchCategoryDTO Search):IRequest<GetAllCategoryQueryResponse>;
public record GetAllCategoryQueryResponse(List<ShowCategoryDTO> categories,SearchCategoryDTO Search,int pageCount);