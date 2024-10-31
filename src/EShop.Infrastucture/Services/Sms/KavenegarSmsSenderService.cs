using EShop.Application.Contracts.Services;
using EShop.Application.Model;
using Kavenegar;
using Microsoft.Extensions.Options;

namespace EShop.Infrastucture.Services.Sms;

public class KavenegarSmsSenderService(IOptionsMonitor<SiteSettings> options) : ISmsSenderService
{
    private readonly SmsSettings _smsConfig = options.CurrentValue.SmsSettings;

    public async Task<bool> SendOTP(string receptor, string templateName, string token1, string? token2 = "", string? token3 = "")
    {
        try
        {
            var api = new KavenegarApi(_smsConfig.ApiKey);
            await api.VerifyLookup(receptor, token1, token2, token3, templateName);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SendPublic(string receptor, string message)
    {
        try
        {
            var api = new KavenegarApi(_smsConfig.ApiKey);
            await api.Send(_smsConfig.Sender, receptor, message);
            return true;
        }
        catch
        {
            return false;
        }
    }
}