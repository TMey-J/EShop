namespace EShop.Domain.Entities.Mongodb;

public class MongoProductFeature:MongoBaseEntity
{
    public long ProductId { get; set;}
    public string FeatureName {get; set;}=string.Empty;
    public string FeatureValue {get; set;}=string.Empty;
}