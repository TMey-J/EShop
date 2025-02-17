using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities;

public class Comment:BaseEntity
{
    public long ProductId { get; set; }
    [Required]
    [MaxLength(int.MaxValue)]
    public string Body { get; set; }=string.Empty;
    
    [Required]
    [MinLength(1), MaxLength(5)]
    public byte Rating { get; set; }
    
    public long? ParentId { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime ModifiedDateTime { get; set; }

    #region Relations

    public ICollection<Comment>? Replies { get; set; }
    public Comment? ParentComment { get; set; }
    public Product? Product { get; set; }

    #endregion

}