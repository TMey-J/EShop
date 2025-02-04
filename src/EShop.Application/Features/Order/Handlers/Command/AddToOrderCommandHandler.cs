using EShop.Application.Features.Order.Requests.Command;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.Order.Handlers.Command;

public class AddToOrderCommandHandler(
    ISellerProductRepository sellerProductRepository,
    IOrderRepository orderRepository,
    IOrderDetailRepository orderDetailRepository,
    IRabbitmqPublisherService rabbitmqPublisherService) :
    IRequestHandler<AddToOrderCommandRequest, AddToOrderCommandResponse>
{
    private readonly ISellerProductRepository _sellerProductRepository = sellerProductRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository = orderDetailRepository;
    private readonly IRabbitmqPublisherService _rabbitmqPublisherService = rabbitmqPublisherService;

    public async Task<AddToOrderCommandResponse> Handle(AddToOrderCommandRequest request,
        CancellationToken cancellationToken)
    {
        if (!await _sellerProductRepository.IsExistAsync(request.SellerId,
                request.ProductId,
                request.ColorId))
        {
            throw new CustomBadRequestException(["این محصول یافت نشد"]);
        }
        
        var order = await _orderRepository.GetOpenOrderByUserIdAsync(request.UserId);
        if (order is null)
        {
            order = new Domain.Entities.Order
            {
                UserId = request.UserId,
                OrderDetails = new List<OrderDetail>
                {
                    new()
                    {
                        ProductId = request.ProductId,
                        ColorId = request.ColorId,
                        SellerId = request.SellerId,
                        Count = request.Quantity
                    }
                }
            };
            await using var transaction = await _orderRepository.BeginTransactionAsync();
            try
            {
                await _orderRepository.CreateAsync(order);
                await _orderRepository.SaveChangesAsync();
                var mongoOrder = new MongoOrder
                {
                    Id = order.Id,
                    UserId = order.UserId,
                };
                await _rabbitmqPublisherService.PublishMessageAsync<MongoOrder>(new(ActionTypes.Create, mongoOrder),
                    RabbitmqConstants.QueueNames.Order, RabbitmqConstants.RoutingKeys.Order);
                
                var mongoOrderDetail = new MongoOrderDetail
                {
                    Id=order.OrderDetails.First().Id,
                    ProductId = request.ProductId,
                    ColorId = request.ColorId,
                    Count = request.Quantity,
                    OrderId = order.Id,
                    SellerId = request.SellerId
                };
                await _rabbitmqPublisherService.PublishMessageAsync<MongoOrderDetail>(new(ActionTypes.Create, mongoOrderDetail),
                    RabbitmqConstants.QueueNames.OrderDetail, RabbitmqConstants.RoutingKeys.OrderDetail);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
        }
        else
        {
            var orderDetails = await _orderDetailRepository.GetAllOrderDetailsByOrderIdAsync(order.Id);
            var orderDetail = orderDetails.SingleOrDefault(o =>
                o.ProductId == request.ProductId && o.ColorId == request.ColorId &&
                o.SellerId == request.SellerId);
            await using var transaction = await _orderDetailRepository.BeginTransactionAsync();
            try
            {
                
                if (orderDetail is null)
                {
                    orderDetail = new OrderDetail
                    {
                        ProductId = request.ProductId,
                        ColorId = request.ColorId,
                        SellerId = request.SellerId,
                        Count = request.Quantity,
                        OrderId = order.Id
                    };
                    await _orderDetailRepository.CreateAsync(orderDetail);
                    await _orderDetailRepository.SaveChangesAsync();

                    var mongoOrderDetail = new MongoOrderDetail
                    {
                        Id=orderDetail.Id,
                        ProductId = request.ProductId,
                        ColorId = request.ColorId,
                        Count = request.Quantity,
                        OrderId = order.Id,
                        SellerId = request.SellerId
                    };
                    await _rabbitmqPublisherService.PublishMessageAsync<MongoOrderDetail>(new(ActionTypes.Create, mongoOrderDetail),
                        RabbitmqConstants.QueueNames.OrderDetail, RabbitmqConstants.RoutingKeys.OrderDetail);
                }
                else
                {
                    await _orderDetailRepository.ChangeCountOfOrderDetailAsync(orderDetail.Id, request.Quantity);
                    var mongoOrderDetail = new MongoOrderDetail
                    {
                        Id=orderDetail.Id,
                        ProductId = request.ProductId,
                        ColorId = request.ColorId,
                        Count = request.Quantity,
                        OrderId = order.Id,
                        SellerId = request.SellerId
                    };
                    await _rabbitmqPublisherService.PublishMessageAsync<MongoOrderDetail>(new(ActionTypes.Update, mongoOrderDetail),
                        RabbitmqConstants.QueueNames.OrderDetail, RabbitmqConstants.RoutingKeys.OrderDetail);
                }
                
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
        }

        return new AddToOrderCommandResponse();
    }
}