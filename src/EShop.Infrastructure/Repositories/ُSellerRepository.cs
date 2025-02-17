using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories
{
    public class SellerRepository(SQLDbContext context) : GenericRepository<Seller>(context), ISellerRepository
    {
        private readonly DbSet<Seller> _seller = context.Set<Seller>();
        public async Task<Seller?> FindByIdWithIncludeTypeOfPerson(long sellerId)
        {
            return await _seller.Include(x=>x.LegalSeller)
                .Include(x=>x.IndividualSeller).Include(x=>x.User)
                .SingleOrDefaultAsync(x=>x.Id == sellerId);
        }
    }
}