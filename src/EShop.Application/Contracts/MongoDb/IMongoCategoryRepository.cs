using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Contracts.MongoDb
{
    public interface IMongoCategoryRepository:IMongoGenericRepository<MongoCategory>
    {
        Task<GetAllCategoryQueryResponse> GetAllAsync(SearchCategoryDto search);
        Task<List<MongoFeature>> GetCategoryFeatures(long categoryId);
        Task<List<string>> GetCategoryHierarchyAsync(long categoryId);
    }
}
