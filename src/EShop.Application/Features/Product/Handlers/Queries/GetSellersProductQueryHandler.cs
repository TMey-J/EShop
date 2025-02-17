using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.Product.Requests.Queries;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.Product.Handlers.Queries;

public class GetSellersProductQueryHandler(IMongoProductRepository productRepository,IMongoColorRepository colorRepository,
    IMongoSellerProductRepository sellerProductRepository) :
    IRequestHandler<GetSellersProductQueryRequest, GetSellersProductQueryResponse>
{
    private readonly IMongoProductRepository _product = productRepository;
    private readonly IMongoColorRepository _colorRepository = colorRepository;
    private readonly IMongoSellerProductRepository _sellerProductRepository = sellerProductRepository;

    public async Task<GetSellersProductQueryResponse> Handle(GetSellersProductQueryRequest request,
        CancellationToken cancellationToken)
    {
        var product=await _product.FindByIdAsync(request.ProductId)
            ?? throw new NotFoundException(NameToReplaceInException.Product);
        var colorCode = request.Code;
        if (!colorCode.StartsWith('#'))
        {
            colorCode= '#' + request.Code;
        }
        var color=await _colorRepository.FindByAsync(nameof(MongoColor.Code),colorCode)
            ?? throw new NullReferenceException(NameToReplaceInException.Color);
        var sellers=await _sellerProductRepository.GetAllByProductAndColorIdAsync(product.Id, color.Id);
        
        return new GetSellersProductQueryResponse(sellers);
    }
}