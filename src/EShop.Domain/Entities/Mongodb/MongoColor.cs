namespace EShop.Domain.Entities.Mongodb;

public class MongoColor:MongoBaseEntity
{
    public string Name { get; set; }=string.Empty;
    
    public string Code { get; set; }=string.Empty;
}