using MongoDB.Bson.Serialization.Attributes;

namespace EShop.Domain.Entities.Mongodb
{
    public class MongoCategory:MongoBaseEntity
    {
        public string Title { get; set; }=string.Empty;
        public long? ParentId { get; set; }
        public string? Picture { get; set; }
    }
}
