namespace EShop.Domain.Entities.Mongodb;

public class MongoColor:MongoBaseEntity
{
    public string ColorName { get; set; }=string.Empty;
    
    public string ColorCode { get; set; }=string.Empty;
}