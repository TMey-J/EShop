using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EShop.Domain.Entities.Mongodb;

public class MongoProduct : MongoBaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string EnglishTitle { get; set; } = string.Empty;
    public uint BasePrice { get; set; }
    public byte DiscountPercentage { get; set; }
    public long SellerId { get; set; }
    public string CategoryTitle { get; set; } = string.Empty;
    public long CategoryId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime? EndOfDiscount { get; set; }
    public Dictionary<string, string> Colors { get; set; } = [];
    public List<string> Tags { get; set; } = [];
    public List<string> Images { get; set; } = [];

    [BsonIgnore] public MongoSellerProduct? SellerProduct { get; set; }
};

public record MongoSellerProduct
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; }

    public long SellerId { get; set; }
    public long ProductId { get; set; }
    public int Count { get; set; }
};