using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Feature.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.Feature.Handlers.Queries;

public class GetFeatureQueryHandler(IMongoFeatureRepository feature):
    IRequestHandler<GetFeatureQueryRequest,GetFeatureQueryResponse>
{
    private readonly IMongoFeatureRepository _feature = feature;

    public async Task<GetFeatureQueryResponse> Handle(GetFeatureQueryRequest request, CancellationToken cancellationToken)
    {
        var category = await _feature.FindByIdAsync(request.Id)??
                       throw new NotFoundException(NameToReplaceInException.Feature);
        
        return new GetFeatureQueryResponse(category.Id,category.Name);
    }
}