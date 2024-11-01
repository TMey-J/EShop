using EShop.Application.Contracts;
using EShop.Domain.Entities;
using EShop.Infrastucture.Databases;

namespace EShop.Infrastucture.Repositories;

internal class GenericRepository<TEntity>(SQLDbContext context) : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet=context.Set<TEntity>();
    private readonly SQLDbContext _context =context;
    public async Task CreateAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Delete(TEntity entity)
    {
         _dbSet.Remove(entity);
    }

    public async Task<TEntity?> FindByIdAsync(long id)
    {
        return await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
       await _context.SaveChangesAsync();
    }

    public void SoftDeleteAsync(TEntity entity)
    {
        entity.IsDelete = true;
        Update(entity);
    }

    public void Update(TEntity entity)
    {
         _dbSet.Update(entity);
    }
}
