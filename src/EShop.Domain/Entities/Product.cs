using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Domain.Entities;

public class Product : BaseEntity
{
    [Required]
    [MaxLength(300)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(300)]
    public string EnglishTitle { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public double BasePrice { get; set; }
    [Required]
    [Column(TypeName ="ntext")]
    public string Description { get; set; } = string.Empty;
    
    [MaxLength(3)]
    public int DiscountPercentage { get; set; }
    
    public DateTime? EndOfDiscount { get; set; }

    public long CategoryId { get; set; }

    #region Relationships
    public ICollection<Tag>? Tags { get; set; }
    public ICollection<ProductImages>? Images { get; set; }
    public ICollection<Color>? Colors { get; set; }
    public Category? Category { get; set; }
    #endregion
}