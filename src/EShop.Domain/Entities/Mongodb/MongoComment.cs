using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities.Mongodb;

public class MongoComment:MongoBaseEntity
{
    public long ProductId { get; set; }
    public string Body { get; set; }=string.Empty;
    public byte Rating { get; set; }
    public long? ParentId { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime ModifiedDateTime { get; set; }

}