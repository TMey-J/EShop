using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities;

public class ProductColor
{
    public long ProductId { get; set; }
    public long ColorId { get; set; }

    #region Relationships

    public Product Product { get; set; }
    public Color Color { get; set; }

    #endregion
}