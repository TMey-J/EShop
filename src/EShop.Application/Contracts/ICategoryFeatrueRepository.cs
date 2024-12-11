using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Contracts;

public interface ICategoryFeatureRepository
{
    void DeleteAllFeaturesFromCategory(long categoryId);
    Task SaveChangesAsync();
}