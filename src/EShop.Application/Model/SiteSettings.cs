using Microsoft.AspNetCore.Identity;

namespace EShop.Application.Model;

public class SiteSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = null!;
    public string UserDefaultAvatar { get; set; } = null!;
    public int WaitForSendCodeSeconds { get; set; }
    public EmailConfigs EmailConfigs { get; set; } = null!;
    public bool EnableEmailConfirmation { get; set; }
    public TimeSpan EmailConfirmationTokenProviderLifespan { get; set; }
    public PasswordOptions PasswordOptions { get; set; } = null!;
    public LockoutOptions LockoutOptions { get; set; } = null!;
    public CookieOptions CookieOptions { get; set; } = null!;
    public JwtConfigs JwtConfigs { get; set; } = null!;
    public SmsSettings SmsSettings { get; set; } = null!;
    public FilesPath FilesPath { get; set; } = null!;
    public AdminUser AdminUser { get; set; } = null!;
    public Rabbitmq Rabbitmq { get; set; } = null!;
    
    public SystemSeller SystemSeller { get; set; } = null!;
    public string DefaultUserAvatar { get; set; } = null!;
}

public class ConnectionStrings
{
    public string SQLDbContextConnection { get; set; } = null!;
}

public class CookieOptions
{
    public string AccessDeniedPath { get; set; } = null!;
    public string CookieName { get; set; } = null!;
    public TimeSpan ExpireTimeSpan { get; set; }
    public string LoginPath { get; set; } = null!;
    public string LogoutPath { get; set; }=null!;
    public bool SlidingExpiration { get; set; }
}

public class EmailConfigs
{
    public string SiteTitle { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public bool UseSSL { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string LocalWritePath { get; set; } = null!;
    public string AdminEmail { get; set; } = null!;
}

public class JwtConfigs
{
    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int NotBeforeMinutes { get; set; }
    public int ExpirationMinutes { get; set; }
}
public class SmsSettings
{
    public string ApiKey { get; set; } = null!;
    public string Sender { get; set; } = null!;
    public string LoginCodeTemplateName { get; set; } = null!;
    public string LocalWritePath { get; set; } = null!;
}
public class FilesPath
{
    public string Category { get; set; } = null!;
    public string UserAvatar { get; set; } = null!;
    public string SellerLogo { get; set; } = null!;
}
public class AdminUser
{
    public string UserName { get; set; } = null!;
    public string EmailOrPhoneNumber { get; set; }=null!;
    public string Password { get; set; }=null!;
}
public class SystemSeller
{
    public string UserName { get; set; } = null!;
    public string EmailOrPhoneNumber { get; set; }=null!;
    public string Password { get; set; }=null!;
    public string ShopName { get; set; }=string.Empty;
    public string CityName { get; set; }=string.Empty;
    public string PostalCode { get; set; }=string.Empty;
    public string Address { get; set; }=string.Empty;
    public string NationalId { get; set; }=string.Empty;
    public string CartOrShebaNumber { get; set; }=string.Empty;
}
public class Rabbitmq
{
    public string Uri { get; set; } = null!;
    public string ClientProvidedName { get; set; } = null!;
    public string ExchangeName { get; set; } = null!;
    public string RoutingKey { get; set; } = null!;
    public string QueueName { get; set; } = null!;

}