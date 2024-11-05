using EShop.Application.Contracts.Services;
using EShop.Application.Model;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace EShop.Infrastructure.Services.Email;

public class EmailSenderService(IOptionsSnapshot<SiteSettings> siteSettings) : IEmailSenderService
{
    private readonly EmailConfigs _emailConfigs = siteSettings.Value.EmailConfigs;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
       var mimeMessage=new MimeMessage()
       {
           Subject = subject,
           Date = DateTime.Now,
           Sender=new MailboxAddress(_emailConfigs.SiteTitle,_emailConfigs.UserName),
           Body=new TextPart(TextFormat.Html)
           {
               Text = body
           }     
       };
        mimeMessage.From.Add(new MailboxAddress(_emailConfigs.SiteTitle, _emailConfigs.UserName));
        mimeMessage.To.Add(new MailboxAddress(string.Empty,to));

        using var client = new SmtpClient();
        await client.ConnectAsync(_emailConfigs.Host, _emailConfigs.Port, _emailConfigs.UseSSL);
        await client.AuthenticateAsync(_emailConfigs.UserName,_emailConfigs.Password).ConfigureAwait(false);
        await client.SendAsync(mimeMessage).ConfigureAwait(false);
        await client.DisconnectAsync(true).ConfigureAwait(false);
    }
}
