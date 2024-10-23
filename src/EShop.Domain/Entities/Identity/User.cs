using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities.Identity
{
    public class User: IdentityUser<long>
    {
        [MaxLength(11)]
        public override string? PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
        #region Relations

        public virtual ICollection<UserClaim>? UserClaims { get; set; }
        public virtual ICollection<UserLogin>? UserLogins { get; set; }
        public virtual ICollection<UserToken>? UserTokens { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; }

        #endregion Relations
    }
}
