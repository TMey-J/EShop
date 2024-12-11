namespace EShop.Domain.Entities;

public class CategoryFeature
{
    public long CategoryId { get; set; }
    public long FeatureId { get; set; }

    #region Relations

    public Category? Category { get; set; }
    public Feature? Feature { get; set; }

    #endregion
}