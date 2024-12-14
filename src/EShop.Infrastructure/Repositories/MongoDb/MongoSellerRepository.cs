using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;
using EShop.Application.Features.AdminPanel.User.Requests.Queries;
using EShop.Application.Model;
using EShop.Infrastructure.Databases;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Tag = EShop.Domain.Entities.Tag;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoSellerRepository(MongoDbContext mongoDb) : MongoGenericRepository<Seller>(mongoDb),IMongoSellerRepository
    {
        private readonly IMongoCollection<Seller> _user = mongoDb.GetCollection<Seller>();
    }
}