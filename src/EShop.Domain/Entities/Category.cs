using Microsoft.EntityFrameworkCore;

namespace EShop.Domain.Entities
{
    public class Category:BaseEntity
    {
        public string Title { get; set; }=string.Empty;
        public HierarchyId Parent { get; set; }
        public string? Picture { get; set; }
    }
}
