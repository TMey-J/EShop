using MongoDB.Bson.Serialization.Attributes;

namespace EShop.Domain.Entities.Mongodb;

public class MongoCity:MongoBaseEntity
{
    public string Title { get; set; }=string.Empty; 
    public long ProvinceId { get; set; }
    
    [BsonIgnore]
    public MongoProvince Province { get; set; }
}