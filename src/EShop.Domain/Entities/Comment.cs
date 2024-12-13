using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Domain.Entities;

public class Comment:BaseEntity
{
    public long ProductId { get; set; }
    [Required]
    [Column(TypeName ="ntext")]
    public string Description { get; set; }=string.Empty;
    
    [Required]
    [MinLength(1), MaxLength(5)]
    public byte Rating { get; set; }
    
    public long? ParentId { get; set; }

    #region Relations

    public ICollection<Comment>? Replies { get; set; }
    public Comment? ParentComment { get; set; }
    public Product? Product { get; set; }

    #endregion

}