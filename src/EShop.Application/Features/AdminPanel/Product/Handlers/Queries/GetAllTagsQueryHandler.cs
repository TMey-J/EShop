using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Product.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.Product.Handlers.Queries;

public class GetAllProductsQueryHandler(IMongoProductRepository product):
    IRequestHandler<GetAllProductsQueryRequest,GetAllProductQueryResponse>
{
    private readonly IMongoProductRepository _product = product;

    public async Task<GetAllProductQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
    {
        var tags = await _product.GetAllAsync(request.Search);
        
        return tags;
    }
}