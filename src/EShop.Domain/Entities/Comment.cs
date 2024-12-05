using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EShop.Domain.Entities;

public class Comment:BaseEntity
{
    [Required]
    [Column(TypeName ="ntext")]
    public string Description { get; set; }=string.Empty;
    
    [Required]
    [MinLength(1), MaxLength(5)]
    public byte Rating { get; set; }
    
    public HierarchyId Replay { get; set; } = HierarchyId.GetRoot();

    #region Relations

    public Product? Product { get; set; }

    #endregion

}