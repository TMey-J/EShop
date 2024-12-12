using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Feature.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.Feature.Handlers.Queries;

public class GetAllFeaturesQueryHandler(IMongoFeatureRepository feature):
    IRequestHandler<GetAllFeaturesQueryRequest,GetAllFeaturesQueryResponse>
{
    private readonly IMongoFeatureRepository _feature = feature;

    public async Task<GetAllFeaturesQueryResponse> Handle(GetAllFeaturesQueryRequest request, CancellationToken cancellationToken)
    {
        if (request.Search is null)
        {
            var features = await _feature.GetAllAsync();
            var showFeatures = features.Select(x => new ShowFeatureDto(x.Id, x.Name)).ToList();
            return new GetAllFeaturesQueryResponse(showFeatures,null,null);
        }

        var featuresWithSearch = await _feature.GetAllAsync(request.Search);

        return featuresWithSearch;
    }
}