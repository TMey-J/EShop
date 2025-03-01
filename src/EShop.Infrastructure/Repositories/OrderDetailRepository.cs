using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories
{
    public class OrderDetailRepository(SQLDbContext context) : GenericRepository<OrderDetail>(context), IOrderDetailRepository
    {
        private readonly DbSet<OrderDetail> _orderDetail = context.Set<OrderDetail>();

        public async Task ChangeCountOfOrderDetailAsync(long orderDetailId, short count)
        {
            await _orderDetail.Where(x =>
                    x.Id==orderDetailId)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.Count, count));
        }

        public async Task<List<OrderDetail>> GetAllOrderDetailsByOrderIdAsync(long orderId)
        {
            return await _orderDetail.Where(x => x.OrderId == orderId).ToListAsync();
        }

        public async Task<List<OrderDetail>> GetAllOrderDetailsByOrderIdIncludeSellerProductAsync(long orderId)
        {
            return await _orderDetail
                .Include(x=>x.SellerProduct)
                .Where(x => x.OrderId == orderId).ToListAsync();

        }
    }
}