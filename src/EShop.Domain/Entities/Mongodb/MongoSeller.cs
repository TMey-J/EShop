namespace EShop.Domain.Entities.Mongodb;

public class MongoSeller : MongoBaseEntity
{
    public long UserId { get; set; }
    public string UserName { get; set; }=string.Empty;
    public bool IsLegalPerson { get; set; }
    public string ShopName { get; set; }=string.Empty;
    public string? Logo { get; set; }
    public string? Website { get; set; }
    public long CityId { get; set; }
    public string PostalCode { get; set; }=string.Empty;
    public string Address { get; set; }=string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDateTime { get; set; }=DateTime.Now;
    public DocumentStatus DocumentStatus { get; set; }
    public string? RejectReason { get; set; }
    public MongoCity City { get; set; }
    public MongoIndividualSeller? IndividualSeller { get; set; }
    public MongoLegalSeller? LegalSeller { get; set; }
}