using EShop.Application.Contracts;
using EShop.Domain.Entities;
using System.Linq;
using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories
{
    public class TagRepository(SQLDbContext context) : GenericRepository<Tag>(context), ITagRepository
    {
        private readonly DbSet<Tag> _category = context.Set<Tag>();
    }
}