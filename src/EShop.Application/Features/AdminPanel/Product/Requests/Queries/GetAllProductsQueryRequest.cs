namespace EShop.Application.Features.AdminPanel.Product.Requests.Queries;

public record GetAllProductsQueryRequest(SearchProductDto Search):IRequest<GetAllProductQueryResponse>;
public record GetAllProductQueryResponse(List<ShowAllProductForAdminPanelDto> Products,SearchProductDto Search,int PageCount);