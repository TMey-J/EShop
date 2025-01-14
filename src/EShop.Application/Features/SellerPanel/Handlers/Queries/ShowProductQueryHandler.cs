using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.SellerPanel.Requests.Queries;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.SellerPanel.Handlers.Queries;

public class ShowProductQueryHandler(
    IMongoProductRepository productRepository,IMongoCategoryRepository categoryRepository)
    : IRequestHandler<ShowProductQueryRequest, ShowProductQueryResponse>
{
    private readonly IMongoProductRepository _productRepository = productRepository;
    private readonly IMongoCategoryRepository _categoryRepository = categoryRepository;

    public async Task<ShowProductQueryResponse> Handle(ShowProductQueryRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.Id)
            ?? throw new NotFoundException(NameToReplaceInException.Product);
        var category = await _categoryRepository.FindByAsync(nameof(MongoCategory.Title), product.CategoryTitle)
            ?? throw new NotFoundException(NameToReplaceInException.Category);
        var categoryFeatures = await _categoryRepository.GetCategoryFeatures(category.Id);
        var model = new ShowProductForSellerPanelDto
        {
            Id = product.Id,
            Title = product.Title,
            EnglishTitle = product.EnglishTitle,
            CategoryTitle = product.CategoryTitle,
            Image = product.Images.First(),
            Features = categoryFeatures.Select(x => x.Name).ToList()
        };
        return new ShowProductQueryResponse(model);
    }
}