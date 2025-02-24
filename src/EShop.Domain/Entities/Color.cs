﻿using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities;

public class Color:BaseEntity
{
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }=string.Empty;
    
    [Required]
    [MaxLength(7)]
    public string Code { get; set; }=string.Empty;
    
    #region Relationships
    public ICollection<SellerProduct> SellerProducts { get; set; }

    #endregion
}