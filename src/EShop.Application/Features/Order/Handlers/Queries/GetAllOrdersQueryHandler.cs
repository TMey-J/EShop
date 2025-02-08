using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.Order.Requests.Queries;

namespace EShop.Application.Features.Order.Handlers.Queries;

public class GetAllOrdersQueryHandler(
    IMongoOrderRepository orderRepository,
    IMongoOrderDetailRepository orderDetailRepository) :
    IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
{
    private readonly IMongoOrderRepository _orderRepository = orderRepository;
    private readonly IMongoOrderDetailRepository _orderDetailRepository = orderDetailRepository;

    public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQueryRequest request,
        CancellationToken cancellationToken)
    {
            var order=await _orderRepository.FindUserUnpaidOrderAsync(request.UserId)
                ?? throw new CustomBadRequestException(["هیچ سفارشی ثبت نشده"]);

            var orders = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(order.Id);
            
            var totalSum=MathHelper.CalculateTotalSum(orders.Select(x=>(int)x.PriceWithDiscount).ToList());
            
            return new GetAllOrdersQueryResponse(orders, totalSum);

    }
}