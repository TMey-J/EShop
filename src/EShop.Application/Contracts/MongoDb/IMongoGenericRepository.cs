namespace EShop.Application.Contracts.MongoDb;

public interface IMongoGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task CreateAsync(TEntity entity);   
    Task Update(TEntity entity);
    Task Delete(TEntity entity);
    Task SoftDeleteAsync(TEntity entity);
    Task<TEntity?> FindByIdAsync(long id);
    Task<IEnumerable<TEntity>> GetAllAsync();
}