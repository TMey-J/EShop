using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.SellerPanel.Requests.Queries;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.SellerPanel.Handlers.Queries;

public class ShowProductQueryHandler(
    IMongoProductRepository productRepository)
    : IRequestHandler<ShowProductForSellerPanelQueryRequest, ShowProductQueryResponse>
{
    private readonly IMongoProductRepository _productRepository = productRepository;

    public async Task<ShowProductQueryResponse> Handle(ShowProductForSellerPanelQueryRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.Id)
            ?? throw new NotFoundException(NameToReplaceInException.Product);
        var model = new ShowProductForSellerPanelDto
        {
            Id = product.Id,
            Title = product.Title,
            Image = product.Images.First(),
        };
        return new ShowProductQueryResponse(model);
    }
}