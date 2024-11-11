using EShop.Application.Contracts.Services;
using EShop.Application.Model;
using Microsoft.Extensions.Options;

namespace EShop.Infrastructure.Services.Sms;

public class LocalSmsSenderService(IOptionsMonitor<SiteSettings> options) : ISmsSenderService
{
    private readonly SmsSettings _smsConfig = options.CurrentValue.SmsSettings;

    public async Task<bool> SendOTP(string receptor, string templateName, string token1, string? token2 = "", string? token3 = "")
    {
        var body = $"""
                    Receptor : {receptor}
                    Template : {templateName}
                    Token1   : {token1}
                    Token2   : {token2}
                    Token3   : {token3}
                    """;

        var smsPath = Path.Combine(Directory.GetCurrentDirectory(), _smsConfig.LocalWritePath);
        if (!Path.Exists(smsPath))
            Directory.CreateDirectory(smsPath);
        await File.WriteAllTextAsync(smsPath + $"/{Guid.NewGuid():N}-OTP.txt", body);

        return true;
    }

    public async Task<bool> SendPublic(string receptor, string message)
    {
        var body = $"""
                    Receptor : {receptor}
                    Message  : {message}
                    """;

        var smsPath = Path.Combine(Directory.GetCurrentDirectory(), _smsConfig.LocalWritePath, $"{Guid.NewGuid():N}-Public.txt");
        if (!Path.Exists(smsPath))
            Directory.CreateDirectory(smsPath);
        await File.WriteAllTextAsync(smsPath + $"/{Guid.NewGuid():N}-OTP.txt", body);
        return true;
    }
}