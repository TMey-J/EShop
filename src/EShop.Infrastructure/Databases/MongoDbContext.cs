using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace EShop.Infrastructure.Databases;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration config)
    {
        var client = new MongoClient(config.GetConnectionString("MongoDbConnection"));
        _database = client.GetDatabase(config.GetSection("MongoDb:DatabaseName").Value);
    }
    public IMongoCollection<TEntity> GetCollection<TEntity>(string? collectionName = null)
    {
        return _database.GetCollection<TEntity>(collectionName??typeof(TEntity).Name.ToLower());
    }
}