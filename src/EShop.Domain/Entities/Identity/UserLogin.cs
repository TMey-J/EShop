using Microsoft.AspNetCore.Identity;

namespace EShop.Domain.Entities.Identity;

public class UserLogin : IdentityUserLogin<long>
{
    public virtual User User { get; set; }
}