using System.ComponentModel.DataAnnotations;
using EShop.Domain.Entities.Identity;

namespace EShop.Domain.Entities
{
    public class Order : BaseEntity
    {
        public long UserId { get; set; }

        public long TotalSum { get; set; }
        
        public bool IsPayed { get; set; }

        #region Relationships

        public User User { get; set; }

        #endregion
    }
}