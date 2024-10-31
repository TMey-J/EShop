using EShop.Application.Contracts.Services;
using EShop.Application.Model;
using MimeKit.Text;
using MimeKit;
using Microsoft.Extensions.Options;

namespace EShop.Infrastucture.Services;

public class LocalEmailSenderService(IOptionsSnapshot<SiteSettings> siteSettings) : IEmailSenderService
{
    private readonly EmailConfigs _emailConfigs = siteSettings.Value.EmailConfigs;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var mimeMessage = new MimeMessage()
        {
            Subject = subject,
            Date = DateTime.Now,
            Sender = new MailboxAddress(_emailConfigs.SiteTitle, _emailConfigs.UserName),
            Body = new TextPart(TextFormat.Html)
            {
                Text = body
            }
        };
        mimeMessage.From.Add(new MailboxAddress(_emailConfigs.SiteTitle, _emailConfigs.UserName));
        mimeMessage.To.Add(new MailboxAddress(string.Empty, to));

        string emailPath = Path.Combine(Directory.GetCurrentDirectory(), _emailConfigs.LocalWritePath);
        if (!Path.Exists(emailPath))
            Directory.CreateDirectory(emailPath);
        await using var stream = new FileStream(emailPath + $"/{Guid.NewGuid():N}.eml", FileMode.CreateNew);
        await mimeMessage.WriteToAsync(stream);
    }
}
