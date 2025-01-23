using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.Product.Requests.Queries;

namespace EShop.Application.Features.Product.Handlers.Queries;

public class ShowProductQueryHandler(IMongoProductRepository product,IMongoCategoryRepository categoryRepository) :
    IRequestHandler<ShowProductQueryRequest, ShowProductQueryResponse>
{
    private readonly IMongoProductRepository _product = product;
    private readonly IMongoCategoryRepository _categoryRepository = categoryRepository;

    public async Task<ShowProductQueryResponse> Handle(ShowProductQueryRequest request,
        CancellationToken cancellationToken)
    {
        var product=await _product.FindByIdAsync(request.ProductId)
            ?? throw new NullReferenceException(NameToReplaceInException.Product);
        var categoryHierarchy=await _categoryRepository.GetCategoryHierarchyAsync(product.CategoryId);
        var colors=await _product.GetProductColorsAsync(product.Id);
        var model = new ShowProductDto
        {
            Id = product.Id,
            Title = product.Title,
            EnglishTitle = product.EnglishTitle,
            Description = product.Description,
            Images = product.Images,
            Features = product.Features,
            Tags = product.Tags,
            Categories = categoryHierarchy,
            Colors = colors.ToDictionary(x=>x.ColorName,x=>x.ColorCode)
        };
        return new ShowProductQueryResponse(model);
    }
}