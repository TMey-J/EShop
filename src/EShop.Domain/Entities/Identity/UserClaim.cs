using Microsoft.AspNetCore.Identity;

namespace EShop.Domain.Entities.Identity;

public class UserClaim : IdentityUserClaim<long>
{
    public virtual User User { get; set; }
}