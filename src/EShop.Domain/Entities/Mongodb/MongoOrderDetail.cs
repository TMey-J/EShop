using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EShop.Domain.Entities.Mongodb
{
    public class MongoOrderDetail : MongoBaseEntity
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public long SellerId { get; set; }
        public long ColorId { get; set; }
        public short Count { get; set; }
    }
}