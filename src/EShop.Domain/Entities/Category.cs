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
        public HierarchyId Parent { get; set; }
        [MaxLength(40)]
        public string? Picture { get; set; }
    }
}
