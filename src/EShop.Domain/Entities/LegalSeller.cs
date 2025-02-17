using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EShop.Domain.Entities.Identity;

namespace EShop.Domain.Entities;
public class LegalSeller : BaseEntity
{
    public long SellerId { get; set; }

    [Required]
    [MaxLength(200)]
    public string CompanyName { get; set; }=string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string RegisterNumber { get; set; }=string.Empty;
    
    [Required]
    [MaxLength(12)]
    public string? EconomicCode { get; set; }
    
    [Required]
    [MaxLength(300)]
    public string SignatureOwners { get; set; }=string.Empty;
    
    [Required]
    [MaxLength(24)]
    public string ShabaNumber { get; set; }=string.Empty;

    public CompanyType CompanyType { get; set; }

    #region Relations

    public Seller? Seller { get; set; }

    #endregion
}

public enum CompanyType:byte
{
    [Display(Name = "سهامی عام")]
    PublicStock,

    [Display(Name = "سهامی خاص")]
    PrivateEquity,

    [Display(Name = "مسئولیت محدود")]
    LimitedResponsibility,

    [Display(Name = "تعاونی")]
    Cooperative,

    [Display(Name = "تضامنی")]
    Solidarity
}