using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EShop.Domain.Entities.Mongodb;

public class MongoProduct : MongoBaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string EnglishTitle { get; set; } = string.Empty;
    public string CategoryTitle { get; set; } = string.Empty;
    public long CategoryId { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public List<string> Images { get; set; } = [];
};