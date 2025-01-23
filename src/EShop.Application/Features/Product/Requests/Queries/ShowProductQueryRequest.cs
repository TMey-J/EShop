namespace EShop.Application.Features.Product.Requests.Queries;

public record ShowProductQueryRequest(long ProductId):IRequest<ShowProductQueryResponse>;
public record ShowProductQueryResponse(ShowProductDto Product);