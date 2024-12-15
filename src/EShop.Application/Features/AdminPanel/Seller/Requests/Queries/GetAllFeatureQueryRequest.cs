namespace EShop.Application.Features.AdminPanel.Seller.Requests.Queries;

public record GetAllSellersQueryRequest(SearchSellerDto Search) : IRequest<GetAllSellersQueryResponse>;
public record GetAllSellersQueryResponse(List<ShowSellerDto> Seller,SearchSellerDto Search,int PageCount);