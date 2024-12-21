using EShop.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace EShop.Application.Contracts;

public interface IGenericRepository<TEntity>
{
    Task CreateAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void SoftDeleteAsync(TEntity entity);
    Task<TEntity?> FindByIdAsync(long id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<bool> IsExistsByAsync(string propertyToFilter, object propertyValue, long? id = null);
    Task<TEntity?> FindByAsync(string propertyToFilter, object propertyValue);
    Task<bool> IsAnyAsync();
    Task SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    
    


}
