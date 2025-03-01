using Blogger.Application.Common.Exceptions;
using Dto.Payment;
using ZarinPal.Class;

namespace EShop.Infrastructure.Services.Payment;

public class ZarinPalSandboxPaymentService : IPaymentService
{
    private readonly ZarinPal.Class.Payment _payment = new Expose().CreatePayment();

    public async Task<(string Authority, string GetewayUrl)> Payment(int amount,string merchantId,string callbackUrl,string? email,string? mobile,string? description = null)
    {
        var result = await _payment.Request(new PaymentDto
        {
            Amount = amount,
            MerchantId = merchantId,
            CallbackUrl = callbackUrl,
            Email = email??"taha@gmail.com",
            Mobile = mobile??"09173920575",
            Description = description ?? "Description"
        },ZarinPal.Class.Payment.Mode.sandbox);
        if (result.Authority is null)
        {
            throw new CustomInternalServerException(["Authority Null"]);
        }
        return (result.Authority, $"https://sandbox.zarinpal.com/pg/StartPay/{result.Authority}");
    }

    public async Task<(bool isSuccess, int? refId)> Verify(int amount,string merchantId,string authority)
    {

        var verification = await _payment.Verification(new DtoVerification()
        {
            Amount = amount,
            MerchantId = merchantId,
            Authority = authority,
        },ZarinPal.Class.Payment.Mode.sandbox);

        if(verification.Status is not 100 and not 101)
            return (false, null);

        return (true, verification.RefId);
    }
}