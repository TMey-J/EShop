using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EShop.Domain.Entities.Identity;

namespace EShop.Domain.Entities
{
    public class Order : BaseEntity
    {
        public long UserId { get; set; }

        [NotMapped]
        public long TotalSum { get; set; }
        
        public bool IsPayed { get; set; }

        #region Relationships
        public ICollection<OrderDetail> OrderDetails { get; set; }

        #endregion
    }
}