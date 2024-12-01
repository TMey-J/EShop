using EShop.Application.Contracts.MongoDb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;

namespace EShop.Infrastructure.Repositories.MongoDb;

public class MongoGenericRepository<TEntity>(MongoDbContext mongoDb) : IMongoGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly IMongoCollection<TEntity> _collection = mongoDb.GetCollection<TEntity>();

    public async Task CreateAsync(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task CreateAllAsync(List<TEntity> entity)
    {
        await _collection.InsertManyAsync(entity);
    }

    public async Task Update(TEntity entity)
    {
        var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public async Task Delete(TEntity entity)
    {
        var filter = Builders<TEntity>.Filter.Eq(x=>x.Id,entity.Id);

        await _collection.DeleteOneAsync(filter);
    }

    public async Task SoftDeleteAsync(TEntity entity)
    {
        var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
        var update= Builders<TEntity>.Update.Set(x=>x.IsDelete,true);
        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task<TEntity?> FindByIdAsync(long id)
    {
        var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _collection.Find(_=>true).ToListAsync();
    }
    
}