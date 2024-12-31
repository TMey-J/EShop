using EShop.Application.Features.AdminPanel.Feature.Requests.Queries;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Contracts.MongoDb;

public interface IMongoFeatureRepository : IMongoGenericRepository<MongoFeature>
{
    Task<GetAllFeaturesQueryResponse> GetAllAsync(SearchFeatureDto search);
}