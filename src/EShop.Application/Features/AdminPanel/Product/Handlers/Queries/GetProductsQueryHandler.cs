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
        var categoryFeatures = await _categoryRepository.GetCategoryFeatures(product.CategoryId);
        var productFeatures = await _product.GetProductFeaturesAsync(product.Id);
        var features = productFeatures.Select(x => new ShowProductFeatureDto
        {
            Name = x.Key,
            Value = x.Value,
            IsCategoryFeature = categoryFeatures.Select(c => c.Name).Contains(x.Key)
        }).ToList();
        var responseModel = new ShowProductForAdminPanelDto
        {
            Id = product.Id,
            Title = product.Title,
            EnglishTitle = product.EnglishTitle,
            CategoryId = product.CategoryId,
            Images = product.Images,
            Tags = product.Tags,
            Description = product.Description,
            Features = features
        };
        return new GetProductQueryResponse(responseModel);
    }
}