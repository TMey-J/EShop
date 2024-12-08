using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EShop.Domain.Entities.Identity;

namespace EShop.Domain.Entities;

public class IndividualSeller : BaseEntity
{
    #region Properties

    public long SellerId { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string NationalId { get; set; }=string.Empty;
    
    [Required]
    [MaxLength(24)]
    public string CartOrShebaNumber { get; set; }=string.Empty;
    [Required]
    [Column(TypeName = "ntext")]
    public string AboutSeller { get; set; }=string.Empty;

    #endregion

    #region Relations

    public SellerBase? Seller { get; set; }

    #endregion
}