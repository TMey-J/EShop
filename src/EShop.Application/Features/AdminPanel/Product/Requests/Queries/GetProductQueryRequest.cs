namespace EShop.Application.Features.AdminPanel.Product.Requests.Queries;

public record GetProductQueryRequest(long Id):IRequest<GetProductQueryResponse>;
public record GetProductQueryResponse(ShowProductDto Product);