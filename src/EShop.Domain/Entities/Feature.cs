namespace EShop.Domain.Entities;

public class Feature:BaseEntity
{
    public string Name { get; set; }=string.Empty;
    
    #region Relations

    public ICollection<Category> Categories { get; set; }

    #endregion
}