namespace EShop.Application.Features.Product.Requests.Queries;

public record GetSellersProductQueryRequest(long ProductId,string Code):IRequest<GetSellersProductQueryResponse>;
public record GetSellersProductQueryResponse(List<GetSellersProductDto> Sellers);