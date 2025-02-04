namespace EShop.Application.Contracts
{
    public interface IOrderRepository:IGenericRepository<Order>
    {
        Task<Order?> GetOpenOrderByUserIdAsync(long userId);
    }
}
