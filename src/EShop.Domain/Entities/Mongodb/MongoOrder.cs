using MongoDB.Bson.Serialization.Attributes;

namespace EShop.Domain.Entities.Mongodb
{
    public class MongoOrder : MongoBaseEntity
    {
        public long UserId { get; set; }
        public long TotalSum { get; set; }

        public bool IsPayed { get; set; }
        public int? RefId { get; set; }
        public DateTime? PayDateTime { get; set; }
    }
}