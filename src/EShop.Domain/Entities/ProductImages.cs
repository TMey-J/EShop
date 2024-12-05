using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities
{
    public class ProductImages:BaseEntity
    {
        public long ProductId { get; set; }
        [Required]
        [MaxLength(40)]
        public string ImageName { get; set; }=string.Empty;
        
        #region Relationships

        public Product? Product { get; set; }

        #endregion
    }
}
