using EShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Contracts
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        Task<HierarchyId?> GetLastChildHierarchyIdAsync(Category parentCategory);
        Task<long?> GetParentIdWithHierarchyIdAsync(HierarchyId categoryHierarchyId);
    }
}
