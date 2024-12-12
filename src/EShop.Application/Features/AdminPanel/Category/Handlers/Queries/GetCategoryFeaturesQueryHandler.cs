namespace EShop.Application.Features.AdminPanel.Category.Handlers.Queries;

public class GetCategoryFeaturesQueryRequestHandler(ICategoryRepository category):
    IRequestHandler<GetCategoryFeaturesQueryRequest,GetCategoryFeaturesQueryResponse>
{
    private readonly ICategoryRepository _category = category;

    public async Task<GetCategoryFeaturesQueryResponse> Handle(GetCategoryFeaturesQueryRequest request, CancellationToken cancellationToken)
    {
        var category = await _category.FindByIdWithIncludeFeatures(request.CategoryId)??
                       throw new NotFoundException(NameToReplaceInException.Category);
        
        var features=category.CategoryFeatures?.
            ToDictionary(x=>x.FeatureId,x=>x.Feature?.Name);
        return new GetCategoryFeaturesQueryResponse(features);
    }
}