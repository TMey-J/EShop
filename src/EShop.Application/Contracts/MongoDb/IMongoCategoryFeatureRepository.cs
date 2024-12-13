namespace EShop.Application.Contracts.MongoDb;

public interface IMongoCategoryFeatureRepository
{
    Task CreateAsync(CategoryFeature categoryFeature);
    Task Delete(CategoryFeature categoryFeature);
}