using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities;

public class Feature:BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }=string.Empty;
    
    #region Relations

    public ICollection<CategoryFeature>? CategoryFeatures { get; set; }

    #endregion
}