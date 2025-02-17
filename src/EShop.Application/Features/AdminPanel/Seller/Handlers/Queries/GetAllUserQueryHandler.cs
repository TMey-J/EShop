using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Seller.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.Seller.Handlers.Queries;

public class GetAllSellerQueryHandler(IMongoSellerRepository seller) :
    IRequestHandler<GetAllSellersQueryRequest, GetAllSellersQueryResponse>
{
    private readonly IMongoSellerRepository _seller = seller;

    public async Task<GetAllSellersQueryResponse> Handle(GetAllSellersQueryRequest request, CancellationToken cancellationToken)
    {
        var sellers = await _seller.GetAllAsync(request.Search);

        return sellers;
    }
}