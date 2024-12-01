using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities;

public class City:BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }=string.Empty; 
    
    public long ProvinceId { get; set; }

    #region Relationships
    
    public Province Province { get; set; }

    #endregion
}