using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities
{
    public class Category:BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }=string.Empty;
        
        public long? ParentId { get; set; }

        [MaxLength(40)]
        public string? Picture { get; set; }

        #region Relationships

        public ICollection<Category>? Categories { get; set; }
        public Category? Parent { get; set; }
        
        public ICollection<Product>? Products { get; set; }
        public ICollection<CategoryFeature>? CategoryFeatures { get; set; }

        #endregion
    }
}
