using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EShop.Domain.Entities.Identity;

namespace EShop.Domain.Entities;

public class SellerBase : BaseEntity
{
    public long UserId { get; set; }

    [Required]
    [MaxLength(200)]
    public string ShopName { get; set; }=string.Empty;

    [MaxLength(40)]
    public string? Logo { get; set; }

    [MaxLength(200)]
    public string? Website { get; set; }

    public long CityId { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string PostalCode { get; set; }=string.Empty;
    
    [Required]
    [MaxLength(300)]
    public string Address { get; set; }=string.Empty;

    public bool IsDocumentApproved { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDateTime { get; set; }
    
    public DocumentStatus DocumentStatus { get; set; }

    [Display(Name = "دلایل رد مدارک فروشنده")]
    [Column(TypeName ="ntext")]
    public string? RejectReason { get; set; }
    

    #region Relations

    public User? User { get; set; }

    public City? City { get; set; }
    
    public IndividualSeller? IndividualSeller { get; set; }
    
    public LegalSeller? LegalSeller { get; set; }
    public ICollection<SellerProduct>? SellersProducts { get; set; }

    #endregion
}
public enum DocumentStatus : byte
{
    [Display(Name = "در انتظار تایید اولیه")]
    AwaitingInitialApproval,

    [Display(Name = "تایید شده")]
    Confirmed,

    [Display(Name = "رد شده در ح    الت اولیه")]
    Rejected,

    [Display(Name = "در انتظار تایید فروشنده سیستم")]
    AwaitingApprovalSystemSeller,

    [Display(Name = "رد شده برای فروشنده سیستم")]
    RejectedSystemSeller

}