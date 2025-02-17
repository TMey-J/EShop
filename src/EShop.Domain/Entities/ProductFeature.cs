using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities;

public class ProductFeature:BaseEntity
{
    public long ProductId { get; set;}
    [Required]
    [MaxLength(100)]
    public string FeatureName {get; set;}=string.Empty;
    [Required]
    [MaxLength(500)]
    public string FeatureValue {get; set;}=string.Empty;

    #region Relations

    public Product? Product { get; set; }

    #endregion
    
}