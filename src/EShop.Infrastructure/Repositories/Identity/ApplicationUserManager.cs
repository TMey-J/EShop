using Blogger.Application.Common.Exceptions;
using EShop.Application.Common.Helpers;
using EShop.Application.Contracts.Identity;
using EShop.Application.Features.AdminPanel.User.Requests.Queries;
using EShop.Application.Model;
using EShop.Domain.Entities.Identity;
using EShop.Infrastructure.Databases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EShop.Infrastructure.Repositories.Identity;

public class ApplicationUserManager(
    IUserStore<User> store,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<User> passwordHasher,
    IEnumerable<IUserValidator<User>> userValidators,
    IEnumerable<IPasswordValidator<User>> passwordValidators,
    ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
    IServiceProvider services,
    IOptionsSnapshot<SiteSettings> siteSettings,
    ILogger<UserManager<User>> logger,
    SQLDbContext context)
    : UserManager<User>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger),
    IApplicationUserManager
    
{
    private readonly string _defaultUserAvatar = siteSettings.Value.DefaultUserAvatar;
    private readonly DbSet<User> _user = context.Set<User>();
    private readonly SQLDbContext _context=context;

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

        var users = await userQuery.Select
        (x => new ShowUserDto(x.Id, x.UserName, x.Email ?? x.PhoneNumber, x.IsActive,
            x.Avatar ?? _defaultUserAvatar)).ToListAsync();

        return new GetAllUsersQueryResponse(users, search, pagination.pageCount);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task<User?> FindByPhoneNumberAsync(string phoneNumber)
    {
        return await _user.SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }
}