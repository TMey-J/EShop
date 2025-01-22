using EShop.Application.Contracts;
using EShop.Domain.Entities;
using EShop.Infrastructure.Databases;
using EShop.Application.Common.Helpers;
using Microsoft.EntityFrameworkCore.Storage;

namespace EShop.Infrastructure.Repositories;

public class GenericRepository<TEntity>(SQLDbContext context) : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _entity=context.Set<TEntity>();
    private readonly SQLDbContext _context =context;
    public async Task CreateAsync(TEntity entity)
    {
        await _entity.AddAsync(entity);
    }

    public void Delete(TEntity entity)
    {
         _entity.Remove(entity);
    }

    public async Task<TEntity?> FindByIdAsync(long id)
    {
        return await _entity.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _entity.ToListAsync();
    }

    public async Task<bool> IsAnyAsync()
    {
        return await _entity.AnyAsync();
    }

    public async Task SaveChangesAsync()
    {
       await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public void SoftDeleteAsync(TEntity entity)
    {
        entity.IsDelete = true;
        Update(entity);
    }

    public void Update(TEntity entity)
    {
         _entity.Update(entity);
    }
    public virtual async Task<bool> IsExistsByAsync(string propertyToFilter, object propertyValue, long? id = null)
    {
        var exp = ExpressionHelpers.CreateAnyExpression<TEntity>(propertyToFilter, propertyValue);
        return await _entity.Where(x => id == null || x.Id != id).AnyAsync(exp);
    }

    public virtual async Task<TEntity?> FindByAsync(string propertyToFilter, object propertyValue)
    {
        var exp = ExpressionHelpers.CreateFindByExpression<TEntity>(propertyToFilter, propertyValue);
        return await _entity.SingleOrDefaultAsync(exp);

    }
}
