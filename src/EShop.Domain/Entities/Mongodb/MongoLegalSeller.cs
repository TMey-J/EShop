namespace EShop.Domain.Entities.Mongodb;

public class MongoLegalSeller:MongoBaseEntity
{
    public long SellerId { get; set; }
    public string CompanyName { get; set; }=string.Empty;
    public string RegisterNumber { get; set; }=string.Empty;
    public string? EconomicCode { get; set; }
    public string SignatureOwners { get; set; }=string.Empty;
    public string ShabaNumber { get; set; }=string.Empty;
    public CompanyType CompanyType { get; set; }
}