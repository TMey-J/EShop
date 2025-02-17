namespace EShop.Domain.Entities.Mongodb;

public class MongoBaseEntity
{
    public long Id { get; set; }
    public bool IsDelete { get; set; }
}