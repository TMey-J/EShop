using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities;

public class ProductTag
{
    public long ProductId { get; set; }
    public long TagId { get; set; }

    #region Relationships

    public Product Product { get; set; }
    public Tag Tag { get; set; }

    #endregion
}