using EShop.Application.DTOs;

namespace EShop.Application.Features.AdminPanel.Requests.Queries.Category;

public record GetAllCategoryQueryRequest(SearchCategoryDto Search):IRequest<GetAllCategoryQueryResponse>;
public record GetAllCategoryQueryResponse(List<ShowCategoryDto> categories,SearchCategoryDto Search,int pageCount);