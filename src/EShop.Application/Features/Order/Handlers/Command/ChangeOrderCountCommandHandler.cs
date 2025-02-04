using EShop.Application.Features.Order.Requests.Command;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.Order.Handlers.Command;

public class ChangeOrderCountCommandHandler(
    ISellerProductRepository sellerProductRepository,
    IOrderRepository orderRepository,
    IOrderDetailRepository orderDetailRepository,
    IRabbitmqPublisherService rabbitmqPublisherService) :
    IRequestHandler<ChangeOrderCountCommandRequest, ChangeOrderCountCommandResponse>
{
    private readonly ISellerProductRepository _sellerProductRepository = sellerProductRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository = orderDetailRepository;
    private readonly IRabbitmqPublisherService _rabbitmqPublisherService = rabbitmqPublisherService;

    public async Task<ChangeOrderCountCommandResponse> Handle(ChangeOrderCountCommandRequest request,
        CancellationToken cancellationToken)
    {
        var orderDetail=await _orderDetailRepository.FindByIdAsync(request.OrderDetailId)
            ?? throw new NotFoundException(NameToReplaceInException.Order);

        if (!await _orderRepository.IsOrderBelongToUserAsync(request.UserId, orderDetail.OrderId))
        {
            throw new CustomBadRequestException(["این سفارش متعلق به شما نیست"]);
        }
        
        await using var transaction = await _orderDetailRepository.BeginTransactionAsync();
        try
        {

            await _orderDetailRepository.ChangeCountOfOrderDetailAsync(orderDetail.Id, request.Quantity);
        
            var mongoOrderDetail = new MongoOrderDetail
            {
                Id = orderDetail.Id,
                ProductId = orderDetail.ProductId,
                ColorId = orderDetail.ColorId,
                Count = request.Quantity,
                OrderId = orderDetail.Id,
                SellerId = orderDetail.SellerId
            };
            await _rabbitmqPublisherService.PublishMessageAsync<MongoOrderDetail>(
                new(ActionTypes.Update, mongoOrderDetail),
                RabbitmqConstants.QueueNames.OrderDetail, RabbitmqConstants.RoutingKeys.OrderDetail);
            await transaction.CommitAsync(cancellationToken);

        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        return new ChangeOrderCountCommandResponse();
    }
}