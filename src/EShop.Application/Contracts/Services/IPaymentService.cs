namespace EShop.Application.Contracts.Services;

public interface IPaymentService
{
    Task<(string Authority, string GetewayUrl)> Payment(int amount,string merchantId,string callbackUrl,string? email,string? mobile,string? description = null);

    Task<(bool isSuccess, int? refId)> Verify(int amount,string merchantId,string authority);
}