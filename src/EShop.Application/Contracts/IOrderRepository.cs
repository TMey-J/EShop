namespace EShop.Application.Contracts
{
    public interface IOrderRepository:IGenericRepository<Order>
    {
        Task<Order?> GetOpenOrderByUserIdAsync(long userId);
        Task<bool> IsOrderBelongToUserAsync(long userId, long orderId);
    }
}
