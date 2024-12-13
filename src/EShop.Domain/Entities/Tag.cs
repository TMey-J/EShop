using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities
{
    public class Tag:BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }=string.Empty;
        
        #region Relationships

        public ICollection<Product>? Products { get; set; } = [];

        #endregion
    }
}
