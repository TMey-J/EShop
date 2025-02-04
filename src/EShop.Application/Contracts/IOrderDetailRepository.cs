namespace EShop.Application.Contracts
{
    public interface IOrderDetailRepository:IGenericRepository<OrderDetail>
    {
        Task ChangeCountOfOrderDetailAsync(long orderDetailId, short count);
        Task<List<OrderDetail>> GetAllOrderDetailsByOrderIdAsync(long orderId);
    }
}
