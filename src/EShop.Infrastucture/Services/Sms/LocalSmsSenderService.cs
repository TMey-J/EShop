using EShop.Application.Contracts.Services;
using EShop.Application.Model;
using Microsoft.Extensions.Options;

namespace EShop.Infrastucture.Services.Sms;

public class LocalSmsSenderService(IOptionsMonitor<SiteSettings> options) : ISmsSenderService
{
    private readonly SmsSettings _smsConfig = options.CurrentValue.SmsSettings;

    public async Task<bool> SendOTP(string receptor, string templateName, string token1, string? token2 = "", string? token3 = "")
    {
        string body = $"""
            Receptor : {receptor}
            Template : {templateName}
            Token1   : {token1}
            Token2   : {token2}
            Token3   : {token3}
            """;

        string smsPath = Path.Combine(Directory.GetCurrentDirectory(), _smsConfig.LocalWritePath);
        if (!Path.Exists(smsPath))
            Directory.CreateDirectory(smsPath);
        File.WriteAllText(smsPath + $"/{Guid.NewGuid():N}-OTP.txt", body);

        return true;
    }

    public async Task<bool> SendPublic(string receptor, string message)
    {
        string body = $"""
            Receptor : {receptor}
            Message  : {message}
            """;

        string smsPath = Path.Combine(Directory.GetCurrentDirectory(), _smsConfig.LocalWritePath, $"{Guid.NewGuid():N}-Public.txt");
        if (!Path.Exists(smsPath))
            Directory.CreateDirectory(smsPath);
        File.WriteAllText(smsPath + $"/{Guid.NewGuid():N}-OTP.txt", body);
        return true;
    }
}