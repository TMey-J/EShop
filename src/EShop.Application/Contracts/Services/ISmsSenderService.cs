namespace EShop.Application.Contracts.Services;

public interface ISmsSenderService
{
    Task<bool> SendPublic(string receptor, string message);

    Task<bool> SendOTP(string receptor, string templateName, string token1, string? token2 = "", string? token3 = "");
}