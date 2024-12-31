using EShop.Application.Features.AdminPanel.Category.Requests.Commands;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.AdminPanel.Category.Handlers.Commands;

public class AddFeaturesToCategoryCommandHandler(
    IFeatureRepository feature,
    ICategoryRepository category,
    ICategoryFeatureRepository categoryFeature,
    IRabbitmqPublisherService rabbitmqPublisher)
    : IRequestHandler<AddFeaturesToCategoryCommandRequest, AddFeaturesToCategoryCommandResponse>
{
    private readonly IFeatureRepository _feature = feature;
    private readonly ICategoryRepository _category = category;
    private readonly ICategoryFeatureRepository _categoryFeature = categoryFeature;
    private readonly IRabbitmqPublisherService _rabbitmqPublisher = rabbitmqPublisher;

    public async Task<AddFeaturesToCategoryCommandResponse> Handle(AddFeaturesToCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var category = await _category.FindByIdWithIncludeFeatures(request.CategoryId);
        if (category is null)
        {
            throw new CustomBadRequestException(["دسته بندی مورد نظر یافت نشد"]);
        }
        List<string> errors = [];
        var categoryFeatures = new List<CategoryFeature>();
        List<MongoCategoryFeature> mongoCategoryFeature;
        foreach (var featureName in request.FeaturesName)
        {
            var feature= await _feature.FindByAsync(nameof(Domain.Entities.Feature.Name), featureName);
            if (feature is not null)
            {
                categoryFeatures.Add(new CategoryFeature
                {
                    CategoryId = request.CategoryId,
                    FeatureId = feature.Id,
                });
            }
            else
            {
                errors.Add($"ویژگی {featureName} موجود نیست");
            }
        }

        if (errors.Count>0)
        {
            throw new CustomBadRequestException(errors);
        }

        if (category.CategoryFeatures is not null && category.CategoryFeatures.Count != 0)
        {
            mongoCategoryFeature = category.CategoryFeatures.Select(x =>
                new MongoCategoryFeature{CategoryId = x.CategoryId,FeatureId = x.FeatureId}).ToList();
            _categoryFeature.DeleteAllFeaturesFromCategory(request.CategoryId);
            await _categoryFeature.SaveChangesAsync();
            
            foreach (var categoryFeature in mongoCategoryFeature)
            {
                await _rabbitmqPublisher.PublishMessageAsync<MongoCategoryFeature>(
                    new(ActionTypes.Delete, categoryFeature),
                    RabbitmqConstants.QueueNames.CategoryFeature,
                    RabbitmqConstants.RoutingKeys.CategoryFeature);
            }
        }

        category.CategoryFeatures = categoryFeatures;
        _category.Update(category);
        await _category.SaveChangesAsync();
        mongoCategoryFeature = categoryFeatures.
            Select(x=>new MongoCategoryFeature{CategoryId = x.CategoryId,FeatureId = x.FeatureId}).ToList();
        foreach (var categoryFeature in mongoCategoryFeature)
        {
            await _rabbitmqPublisher.PublishMessageAsync<MongoCategoryFeature>(
                new(ActionTypes.Create, categoryFeature),
                RabbitmqConstants.QueueNames.CategoryFeature,
                RabbitmqConstants.RoutingKeys.CategoryFeature);
        }
        
        return new AddFeaturesToCategoryCommandResponse();
    }
}
