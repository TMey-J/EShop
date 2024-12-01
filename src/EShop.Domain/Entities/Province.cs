using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities;

public class Province:BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }=string.Empty;
    

    #region Relationships

    public ICollection<City> Cities { get; set; }

    #endregion
}