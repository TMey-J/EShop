using Microsoft.AspNetCore.Identity;

namespace EShop.Domain.Entities.Identity
{
    public class User: IdentityUser<long>
    {
        #region Relations

        public virtual ICollection<UserClaim>? UserClaims { get; set; }
        public virtual ICollection<UserLogin>? UserLogins { get; set; }
        public virtual ICollection<UserToken>? UserTokens { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; }

        #endregion Relations
    }
}
