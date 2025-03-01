using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Domain.Entities
{
    public class Order : BaseEntity
    {
        public long UserId { get; set; }
        
        public long TotalSum { get; set; }
        
        public bool IsPayed { get; set; }
        public int? RefId { get; set; }
        public DateTime? PayDateTime { get; set; }

        #region Relationships
        public ICollection<OrderDetail> OrderDetails { get; set; }

        #endregion
    }
}