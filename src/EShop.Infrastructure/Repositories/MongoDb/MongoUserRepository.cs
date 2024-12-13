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
    public class MongoUserRepository(MongoDbContext mongoDb,
        IOptionsMonitor<SiteSettings> siteSettings) : IMongoUserRepository
    {
        private readonly string _defaultUserAvatar = siteSettings.CurrentValue.DefaultUserAvatar;
        private readonly IMongoCollection<User> _user = mongoDb.GetCollection<User>();


        public async Task<User?> FindByEmailAsync(string email)
        {
            return await MongoQueryable.SingleOrDefaultAsync(
                _user.AsQueryable().Where(x => x.Email == email));
        }

        public async Task<(User?, bool)> FindByEmailOrPhoneNumberWithCheckIsEmailAsync(string emailOrPhoneNumber)
    {
        var isEmail = emailOrPhoneNumber.IsEmail();
        var user = isEmail ?
            await FindByEmailAsync(emailOrPhoneNumber) :
            await FindByPhoneNumberAsync(emailOrPhoneNumber);
        return (user,isEmail);
    }

    public async Task<GetAllUsersQueryResponse> GetAllWithFiltersAsync(SearchUserDto search)
    {
        var userQuery = _user.AsQueryable().IgnoreQueryFilters();

        #region Search

        userQuery = userQuery.CreateContainsExpression(nameof(User.UserName), search.UserName);
        userQuery = userQuery.CreateContainsExpression(nameof(User.Email), search.Email);
        userQuery = userQuery.CreateContainsExpression(nameof(User.PhoneNumber), search.PhoneNumber);

        #endregion

        #region Sort

        userQuery = userQuery.CreateOrderByExperssion(search.SortingBy.ToString(), search.SortingAs);

        userQuery = userQuery.CreateDeleteStatusExperssion(nameof(BaseEntity.IsDelete), search.DeleteStatus);

        userQuery = search.ActivationStatus switch
        {
            ActivationStatus.OnlyActive => userQuery.Where(x => x.IsActive),
            ActivationStatus.False => userQuery.Where(x => !x.IsActive),
            _ => userQuery
        };
        #endregion

        #region Paging

        (IQueryable<User> query, int pageCount) pagination =
            userQuery.Page(search.Pagination.CurrentPage, search.Pagination.TakeRecord);
        userQuery = pagination.query;

        #endregion

        var users = await MongoQueryable.ToListAsync(userQuery.Select
        (x => new ShowUserDto(x.Id, x.UserName, x.Email ?? x.PhoneNumber, x.IsActive,
            x.Avatar ?? _defaultUserAvatar)));

        return new GetAllUsersQueryResponse(users, search, pagination.pageCount);
    }

    public async Task<User?> FindByPhoneNumberAsync(string phoneNumber)
    {
        return await MongoQueryable.SingleOrDefaultAsync(_user.AsQueryable().Where(x => x.PhoneNumber == phoneNumber));
    }

        public async Task CreateAsync(User entity)
        {
            await _user.InsertOneAsync(entity);
        }

        public async Task CreateAllAsync(List<User> entity)
        {
            await _user.InsertManyAsync(entity);
        }

        public async Task Update(User entity)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, entity.Id);
            await _user.ReplaceOneAsync(filter, entity);
        }

        public async Task Delete(User entity)
        {
            var filter = Builders<User>.Filter.Eq(x=>x.Id,entity.Id);

            await _user.DeleteOneAsync(filter);
        }

        public async Task SoftDeleteAsync(User entity)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, entity.Id);
            var update= Builders<User>.Update.Set(x=>x.IsDelete,true);
            await _user.UpdateOneAsync(filter, update);
        }

        public async Task<User?> FindByIdAsync(long id)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            return await _user.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _user.Find(_=>true).ToListAsync();
        }
    }
}