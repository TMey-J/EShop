using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public long SellerId { get; set; }
        public long ColorId { get; set; }
        [Required] public short Count { get; set; }

        #region Relationships

        public SellerProduct SellerProduct { get; set; }
        public Order Order { get; set; }

        #endregion
    }
}