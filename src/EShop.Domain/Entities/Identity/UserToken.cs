using Microsoft.AspNetCore.Identity;

namespace EShop.Domain.Entities.Identity;

public class UserToken : IdentityUserToken<long>
{
    public virtual User User { get; set; }
}