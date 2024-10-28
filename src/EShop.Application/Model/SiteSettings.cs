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
}

public class ConnectionStrings
{
    public string SQLDbContextConnection { get; set; } = null!;
}

public class CookieOptions
{
    public string AccessDeniedPath { get; set; }
    public string CookieName { get; set; }
    public TimeSpan ExpireTimeSpan { get; set; }
    public string LoginPath { get; set; }
    public string LogoutPath { get; set; }
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
}

public class JwtConfigs
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int NotBeforeMinutes { get; set; }
    public int ExpirationMinutes { get; set; }
}