using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Product.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.Product.Handlers.Queries;

public class GetProductQueryHandler(IMongoProductRepository product,IMongoCategoryRepository categoryRepository) :
    IRequestHandler<GetProductQueryRequest, GetProductQueryResponse>
{
    private readonly IMongoProductRepository _product = product;
    private readonly IMongoCategoryRepository _categoryRepository = categoryRepository;

    public async Task<GetProductQueryResponse> Handle(GetProductQueryRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _product.FindByIdAsync(request.Id)
                      ?? throw new NotFoundException(NameToReplaceInException.Product);
        var productFeatures = await _product.GetProductFeaturesAsync(product.Id);
        var responseModel = new ShowProductDto
        {
            Id = product.Id,
            Title = product.Title,
            EnglishTitle = product.EnglishTitle,
            CategoryId = product.CategoryId,
            Images = product.Images,
            Tags = product.Tags,
            Description = product.Description,
            Features = productFeatures
        };
        return new GetProductQueryResponse(responseModel);
    }
}