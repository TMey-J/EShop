
using Microsoft.AspNetCore.Identity;

namespace EShop.Domain.Entities.Identity;

public class Role : IdentityRole<long>
{
    #region Relations

    public virtual ICollection<RoleClaim> RoleClaims { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }

    #endregion Relations
}