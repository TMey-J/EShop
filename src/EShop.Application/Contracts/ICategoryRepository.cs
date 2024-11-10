using EShop.Application.DTOs.Category;
using EShop.Application.Features.AdminPanel.Requests.Queries.Category;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Contracts
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        Task<HierarchyId?> GetLastChildHierarchyIdAsync(Category parentCategory);
        Task<long?> GetParentIdWithHierarchyIdAsync(HierarchyId categoryHierarchyId);
        Task<List<Category>> GetCategoryChildrenAsync(Category category);
        Task<GetAllCategoryQueryResponse> GetAllAsync(SearchCategoryDTO search);
    }
}
