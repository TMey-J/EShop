using EShop.Application.Features.AdminPanel.Feature.Requests.Queries;

namespace EShop.Application.Contracts.MongoDb;

public interface IMongoFeatureRepository : IMongoGenericRepository<Feature>
{
    Task<GetAllFeaturesQueryResponse> GetAllAsync(SearchFeatureDto search);
}