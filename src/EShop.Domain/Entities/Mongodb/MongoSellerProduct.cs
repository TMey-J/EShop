using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EShop.Domain.Entities.Mongodb;

public class MongoSellerProduct
{
    [BsonId][BsonRepresentation(BsonType.String)]
    public string Id { get; set; }
    public long SellerId { get; set; }
    public long ProductId { get; set; }
    public long ColorId { get; set; }
    public short Count { get; set; }
    public uint BasePrice { get; set; }
    public byte DiscountPercentage { get; set; }
    public DateTime? EndOfDiscount { get; set; }
};