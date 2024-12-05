using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities;

public class Color:BaseEntity
{
    [Required]
    [MaxLength(30)]
    public string ColorName { get; set; }=string.Empty;
    
    [Required]
    [MaxLength(7)]
    public string ColorCode { get; set; }=string.Empty;
    
    #region Relationships

    public ICollection<Product>? Products { get; set; }

    #endregion
}