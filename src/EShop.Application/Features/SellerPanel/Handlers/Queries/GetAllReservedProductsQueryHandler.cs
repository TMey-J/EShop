using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.SellerPanel.Requests.Queries;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.SellerPanel.Handlers.Queries;

public class GetAllReservedProductsQueryHandler(
    IMongoSellerProductRepository sellerProductRepository,
    IMongoColorRepository colorRepository)
    : IRequestHandler<GetAllReservedProductsQueryRequest, GetAllReservedProductsQueryResponse>
{
    private readonly IMongoSellerProductRepository _sellerProductRepository = sellerProductRepository;
    private readonly IMongoColorRepository _colorRepository = colorRepository;

    public async Task<GetAllReservedProductsQueryResponse> Handle(GetAllReservedProductsQueryRequest request,
        CancellationToken cancellationToken)
    {
        var products = await _sellerProductRepository
            .GetAllReservedProductsAsync(request.Search, request.SellerId);
        var colors = await _colorRepository
            .GetAllColorsByIdAsync(products.ReservedProducts.Select(x => x.ColorId).ToList());
        foreach (var reserved in products.ReservedProducts)
        {
            reserved.ColorCode = colors.First(x => x.Id == reserved.ColorId).ColorCode;
        }
        return products;
    }
}