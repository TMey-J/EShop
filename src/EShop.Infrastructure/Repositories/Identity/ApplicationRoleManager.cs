using EShop.Application.Contracts.Identity;
using EShop.Domain.Entities.Identity;
using EShop.Infrastructure.Databases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EShop.Infrastructure.Repositories.Identity;

public class ApplicationRoleManager(
    IRoleStore<Role> store,
    IEnumerable<IRoleValidator<Role>> roleValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    ILogger<ApplicationRoleManager> logger,
    SQLDbContext context)
    : RoleManager<Role>(store, roleValidators, keyNormalizer, errors, logger),
    IApplicationRoleManager
{
    private readonly DbSet<Role> _roles=context.Set<Role>();
    public async Task<List<string>> NotExistsRolesNameAsync(List<string> rolesName)
    {
        var roles = await _roles.Select(x=>x.NormalizedName).ToListAsync();
        return rolesName.Where(x => !roles.Contains(x)).ToList();
    }
}