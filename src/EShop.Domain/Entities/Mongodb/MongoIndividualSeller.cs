namespace EShop.Domain.Entities.Mongodb;

public class MongoIndividualSeller:MongoBaseEntity
{
    public long SellerId { get; set; }
    public string NationalId { get; set; }=string.Empty;
    public string CartOrShebaNumber { get; set; }=string.Empty;
    public string AboutSeller { get; set; }=string.Empty;
}