using EShop.Domain.Entities;

namespace EShop.Application.Contracts;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task CreateAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void SoftDeleteAsync(TEntity entity);
    Task<TEntity?> FindByIdAsync(long id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task SaveChangesAsync();


}
