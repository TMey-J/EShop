using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities.Identity
{
    public class User: IdentityUser<long>
    {
        [MaxLength(11)]
        public override string? PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        public DateTime SendCodeLastTime { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(40)]
        public string? Avatar { get; set; }
        [Required]
        [MaxLength(64)]
        public byte[] PasswordSalt { get; set; } = null!;

        #region Relations

        public virtual ICollection<UserClaim>? UserClaims { get; set; }
        public virtual ICollection<UserLogin>? UserLogins { get; set; }
        public virtual ICollection<UserToken>? UserTokens { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; }

        #endregion Relations
    }
}
