using Microsoft.AspNetCore.Identity;

namespace EShop.Application.Common.Helpers;

public static class IdentityHelpers
{
    public static List<string> GetErrors(this IdentityResult identityResult)
        => identityResult.Errors.Select(e => e.Description).ToList();
}
