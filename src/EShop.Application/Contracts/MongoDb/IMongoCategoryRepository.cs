namespace EShop.Application.Contracts.MongoDb
{
    public interface IMongoCategoryRepository:IMongoGenericRepository<Category>
    {
        Task<GetAllCategoryQueryResponse> GetAllAsync(SearchCategoryDto search);
        Task<List<Feature>> GetCategoryFeatures(long categoryId);
    }
}
