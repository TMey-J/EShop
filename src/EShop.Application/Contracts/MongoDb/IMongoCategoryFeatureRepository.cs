using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Contracts.MongoDb;

public interface IMongoCategoryFeatureRepository
{
    Task CreateAsync(MongoCategoryFeature categoryFeature);
    Task Delete(MongoCategoryFeature categoryFeature);
}