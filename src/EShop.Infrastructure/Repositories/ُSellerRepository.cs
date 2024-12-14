using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories
{
    public class SellerRepository(SQLDbContext context) : GenericRepository<Seller>(context), ISellerRepository
    {
        private readonly DbSet<Seller> _seller = context.Set<Seller>();
    }
}