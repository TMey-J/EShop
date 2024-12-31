using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.User.Requests.Queries;
using EShop.Application.Model;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoUserRepository(
        MongoDbContext mongoDb,
        IOptionsMonitor<SiteSettings> siteSettings)
        : MongoGenericRepository<MongoUser>(mongoDb, MongoCollectionsName.User)
            , IMongoUserRepository
    {
        private readonly string _defaultUserAvatar = siteSettings.CurrentValue.DefaultUserAvatar;
        private readonly IMongoCollection<MongoUser> _user = mongoDb.GetCollection<MongoUser>(MongoCollectionsName.User);


        public async Task<MongoUser?> FindByEmailAsync(string email)
        {
            return await MongoQueryable.SingleOrDefaultAsync(
                _user.AsQueryable().Where(x => x.Email == email));
        }

        public async Task<(MongoUser?, bool)> FindByEmailOrPhoneNumberWithCheckIsEmailAsync(string emailOrPhoneNumber)
        {
            var isEmail = emailOrPhoneNumber.IsEmail();
            var user = isEmail
                ? await FindByEmailAsync(emailOrPhoneNumber)
                : await FindByPhoneNumberAsync(emailOrPhoneNumber);
            return (user, isEmail);
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

            (IQueryable<MongoUser> query, int pageCount) pagination =
                userQuery.Page(search.Pagination.CurrentPage, search.Pagination.TakeRecord);
            userQuery = pagination.query;

            #endregion

            var users = await MongoQueryable.ToListAsync(userQuery.Select
            (x => new ShowUserDto(x.Id, x.UserName, x.Email ?? x.PhoneNumber, x.IsActive,
                x.Avatar ?? _defaultUserAvatar)));

            return new GetAllUsersQueryResponse(users, search, pagination.pageCount);
        }

        public async Task<MongoUser?> FindByPhoneNumberAsync(string phoneNumber)
        {
            return await MongoQueryable.SingleOrDefaultAsync(_user.AsQueryable()
                .Where(x => x.PhoneNumber == phoneNumber));
        }
    }
}