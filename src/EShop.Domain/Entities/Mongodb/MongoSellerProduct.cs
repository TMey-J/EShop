using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EShop.Domain.Entities.Mongodb;

public class MongoSellerProduct
{
    [BsonId][BsonRepresentation(BsonType.String)]
    public required string Id { get; set; }
    public long SellerId { get; set; }
    public long ProductId { get; set; }
    public long ColorId { get; set; }
    public short Count { get; set; }
    public uint BasePrice { get; set; }
    public byte DiscountPercentage { get; set; }
    public DateTime? EndOfDiscount { get; set; }
    public CustomMongoProduct Product { get; set; }
};

public class CustomMongoProduct
{
    public string Title { get; set; } = string.Empty;
    public string EnglishTitle { get; set; } = string.Empty;
    public long CategoryId { get; set; }
    public List<string> Images { get; set; } = [];
}