using EShop.Application.Features.Order.Requests.Command;

namespace EShop.Application.Features.Order.Handlers.Command;

public class PayOrderCommandHandler(
    IOrderRepository orderRepository,
    IApplicationUserManager userManager,
    IPaymentService paymentService,
    IOptionsMonitor<SiteSettings> siteSettings) :
    IRequestHandler<PayOrderCommandRequest, PayOrderCommandResponse>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IPaymentService _paymentService = paymentService;
    private readonly PaymentConfig _paymentConfig = siteSettings.CurrentValue.PaymentConfig;

    public async Task<PayOrderCommandResponse> Handle(PayOrderCommandRequest request,
        CancellationToken cancellationToken)
    {
        var order=await _orderRepository.FindByIdAsync(request.OrderId)
            ?? throw new NotFoundException(NameToReplaceInException.Order);
        if (order.UserId != request.UserId)
        {
            throw new CustomBadRequestException(["این سفارش به شما تعلق ندارد"]);
        }

        if (order.RefId > 0 || order.IsPayed)
        {
            throw new CustomBadRequestException(["این یفارش قبلا پرداخت شده است"]);
        }
        var user=await _userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new NotFoundException(NameToReplaceInException.User);
        
        var (authority, getWayUrl) = await _paymentService.Payment((int)order.TotalSum,
            _paymentConfig.MerchantId,request.CallbackUrl,user.Email,user.PhoneNumber);
        
        return new PayOrderCommandResponse(authority, getWayUrl);

    }
}