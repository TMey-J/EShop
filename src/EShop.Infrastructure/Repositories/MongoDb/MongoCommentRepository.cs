using Blogger.Application.Common.Exceptions;
using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Category.Requests.Queries;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoCommentRepository(MongoDbContext mongoDb)
        : MongoGenericRepository<MongoComment>(mongoDb,MongoCollectionsName.Comment),IMongoCommentRepository
    {
        private readonly IMongoCollection<MongoCategory> _category = mongoDb.GetCollection<MongoCategory>(MongoCollectionsName.Comment);
       
    }
}