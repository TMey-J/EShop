using Blogger.Application.Common.Exceptions;
using EShop.Application.Common.Helpers;
using EShop.Application.Contracts.Identity;
using EShop.Domain.Entities.Identity;
using EShop.Infrastucture.Databases;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EShop.Infrastucture.Repositories.Identity;

public class ApplicationUserManager(
    IUserStore<User> store,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<User> passwordHasher,
    IEnumerable<IUserValidator<User>> userValidators,
    IEnumerable<IPasswordValidator<User>> passwordValidators,
    ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
    IServiceProvider services,
    ILogger<UserManager<User>> logger,
    SQLDbContext context)
    : UserManager<User>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger),
    IApplicationUserManager
    
{
    private readonly DbSet<User> _user = context.Set<User>();

    public async Task<User?> FindByPhoneNumberAsync(string phoneNumber)
    {
        return await _user.SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }
    async Task IApplicationUserManager.UpdateUserAsync(User user)
    {
        var update = await base.UpdateAsync(user);
        if (!update.Succeeded)
        {
            throw new CustomInternalServerException(update.GetErrors());
        }
    }
}