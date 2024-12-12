using EShop.Application.Contracts.MongoDb;

namespace EShop.Application.Features.AdminPanel.Category.Handlers.Queries;

public class GetCategoryFeaturesQueryRequestHandler(IMongoCategoryRepository category):
    IRequestHandler<GetCategoryFeaturesQueryRequest,GetCategoryFeaturesQueryResponse>
{
    private readonly IMongoCategoryRepository _category = category;

    public async Task<GetCategoryFeaturesQueryResponse> Handle(GetCategoryFeaturesQueryRequest request, CancellationToken cancellationToken)
    {
        var category = await _category.FindByIdAsync(request.CategoryId)??
                       throw new NotFoundException(NameToReplaceInException.Category);
        
        var features=await _category.GetCategoryFeatures(category.Id);
        var featuresDictionary=features.ToDictionary(x=>x.Id, x=>x.Name);
        return new GetCategoryFeaturesQueryResponse(featuresDictionary);
    }
}