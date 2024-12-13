using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Contracts
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        Task<bool> IsHasChild(Category parentCategory);
        Task<GetAllCategoryQueryResponse> GetAllAsync(SearchCategoryDto search);
        Task<Category?> FindByIdWithIncludeFeatures(long categoryId);
    }
}
