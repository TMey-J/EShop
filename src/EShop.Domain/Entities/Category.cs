using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities
{
    public class Category:BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }=string.Empty;

        [Required]
        public HierarchyId Parent { get; set; } = HierarchyId.GetRoot();

        [MaxLength(40)]
        public string? Picture { get; set; }

        #region Relationships

        public ICollection<Product>? Products { get; set; }
        public ICollection<CategoryFeature>? CategoryFeatures { get; set; }

        #endregion
    }
}
