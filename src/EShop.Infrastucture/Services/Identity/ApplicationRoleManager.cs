using EShop.Application.Contracts.Identity;
using EShop.Domain.Entities.Identity;
using EShop.Infrastucture.Databases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Restaurant.Persistence.Services.Identity;

public class ApplicationRoleManager(
    IRoleStore<Role> store,
    IEnumerable<IRoleValidator<Role>> roleValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    ILogger<ApplicationRoleManager> logger,
    SQLDbContext context)
    : RoleManager<Role>(store,roleValidators,keyNormalizer,errors,logger),
    IApplicationRoleManager
{
   
}