using EShop.Application.Features.Order.Requests.Command;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.Order.Handlers.Command;

public class VerifyOrderPaymentCommandHandler(
    IOrderRepository orderRepository,
    IPaymentService paymentService,
    IOptionsMonitor<SiteSettings> siteSettings,
    IRabbitmqPublisherService rabbitmqPublisherService) :
    IRequestHandler<VerifyOrderPaymentCommandRequest, VerifyOrderPaymentCommandResponse>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IPaymentService _paymentService = paymentService;
    private readonly IRabbitmqPublisherService _rabbitmqPublisherService = rabbitmqPublisherService;
    private readonly PaymentConfig _paymentConfig = siteSettings.CurrentValue.PaymentConfig;

    public async Task<VerifyOrderPaymentCommandResponse> Handle(VerifyOrderPaymentCommandRequest request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindByIdAsync(request.OrderId)
                    ?? throw new NotFoundException(NameToReplaceInException.Order);

        if (order.RefId > 0 || order.IsPayed)
        {
            throw new CustomBadRequestException(["این یفارش قبلا پرداخت شده است"]);
        }

        var (isSuccess, refId) = await _paymentService.Verify((int)order.TotalSum,
            _paymentConfig.MerchantId, request.Authority);
        if (!isSuccess || refId == null)
        {
            throw new CustomInternalServerException(["payment error"]);
        }

        order.IsPayed = true;
        order.RefId = refId;
        order.PayDateTime = DateTime.Now;
        _orderRepository.Update(order);
        await _orderRepository.SaveChangesAsync();
        var mongoOrder = new MongoOrder
        {
            Id = order.Id,
            IsPayed = order.IsPayed,
            RefId = order.RefId,
            TotalSum = order.TotalSum,
            UserId = order.UserId,
            PayDateTime = order.PayDateTime
        };
        await _rabbitmqPublisherService.PublishMessageAsync<MongoOrder>(new(ActionTypes.Update, mongoOrder),
            RabbitmqConstants.QueueNames.Order, RabbitmqConstants.RoutingKeys.Order);
        return new VerifyOrderPaymentCommandResponse(true, refId);
    }
}