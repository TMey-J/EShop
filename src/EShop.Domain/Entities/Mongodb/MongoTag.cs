using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities.Mongodb
{
    public class MongoTag:MongoBaseEntity
    {
        public string Title { get; set; }=string.Empty;

        public bool IsConfirmed { get; set; }
    }
}
