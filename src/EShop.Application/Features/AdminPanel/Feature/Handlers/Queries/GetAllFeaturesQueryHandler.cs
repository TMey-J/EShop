using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Feature.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.Feature.Handlers.Queries;

public class GetAllFeaturesQueryHandler(IMongoFeatureRepository feature):
    IRequestHandler<GetAllFeaturesQueryRequest,GetAllFeaturesQueryResponse>
{
    private readonly IMongoFeatureRepository _feature = feature;

    public async Task<GetAllFeaturesQueryResponse> Handle(GetAllFeaturesQueryRequest request, CancellationToken cancellationToken)
    {
        var features = await _feature.GetAllAsync(request.Search);
        
        return new GetAllFeaturesQueryResponse(features.Features,request.Search,features.PageCount);
    }
}