using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories
{
    public class OrderRepository(SQLDbContext context) : GenericRepository<Order>(context), IOrderRepository
    {
        private readonly DbSet<Order> _orders = context.Set<Order>();

        public async Task<Order?> GetOpenOrderByUserIdAsync(long userId)
        {
            return await _orders.SingleOrDefaultAsync(x => x.UserId == userId && !x.IsPayed);
        }
        
    }
}