using EShop.Application.Contracts.Identity;
using EShop.Application.Contracts.Services;
using EShop.Application.Model;
using EShop.Domain.Entities.Identity;
using EShop.Infrastucture.Databases;
using EShop.Infrastucture.Repositories.Identity;
using EShop.Infrastucture.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Application.Contracts.Identity;
using System.Security.Claims;
using System.Security.Principal;

namespace EShop.Infrastucture
{
    public static class DependencyInjection
    {
        private const string EmailConfirmationTokenProviderName = "ConfirmEmail";

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var siteSettings = configuration.Get<SiteSettings>(configuration.Bind);

            services.AddDataBase(siteSettings.ConnectionStrings.SQLDbContextConnection);

            services.AddIdentityServices();
            services.AddIdentityOptions(siteSettings);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IPrincipal>(provider => provider.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.User ?? ClaimsPrincipal.Current!);
            services.ConfigureServices(environment);
            return services;
        }
        private static IServiceCollection AddDataBase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<SQLDbContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString);
            });
            return services;
        }
        private static void ConfigureServices(this IServiceCollection services,IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.AddKeyedScoped<IEmailSenderService, LocalEmailSenderService>("email");
            }
            else
            {
                services.AddKeyedScoped<IEmailSenderService, EmailSenderService>("email");
            }
        }
        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
            services.AddScoped<UserManager<User>, ApplicationUserManager>();

            services.AddScoped<IApplicationRoleManager, ApplicationRoleManager>();
            services.AddScoped<RoleManager<Role>, ApplicationRoleManager>();

            services.AddScoped<IApplicationSignInManager, ApplicationSignInManager>();
            services.AddScoped<SignInManager<User>, ApplicationSignInManager>();

            services.AddScoped<IJwtService, JwtService>();

            return services;
        }

        private static IServiceCollection AddIdentityOptions(this IServiceCollection services, SiteSettings siteSettings)
        {
            services.AddConfirmEmailDataProtectorTokenOptions(siteSettings);
            services.AddIdentity<User, Role>(identityOptions =>
            {
                SetPasswordOptions(identityOptions.Password, siteSettings);
                SetSignInOptions(identityOptions.SignIn, siteSettings);
                SetUserOptions(identityOptions.User);
                SetLockoutOptions(identityOptions.Lockout, siteSettings);
            }).AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddEntityFrameworkStores<SQLDbContext>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<ConfirmEmailDataProtectorTokenProvider<User>>(EmailConfirmationTokenProviderName);

            services.ConfigureApplicationCookie(identityOptionsCookies =>
            {
                var provider = services.BuildServiceProvider();
                SetApplicationCookieOptions(provider, identityOptionsCookies, siteSettings);
            });

            services.EnableImmediateLogout();

            return services;
        }
        #region Identity Options

        private static void AddConfirmEmailDataProtectorTokenOptions(this IServiceCollection services, SiteSettings siteSettings)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Tokens.EmailConfirmationTokenProvider = EmailConfirmationTokenProviderName;
            });

            services.Configure<ConfirmEmailDataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = siteSettings.EmailConfirmationTokenProviderLifespan;
            });
        }

        private static void EnableImmediateLogout(this IServiceCollection services)
        {
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                // enables immediate logout, after updating the user's stat.
                options.ValidationInterval = TimeSpan.Zero;
                options.OnRefreshingPrincipal = principalContext => { return Task.CompletedTask; };
            });
        }

        private static void SetApplicationCookieOptions(IServiceProvider provider, CookieAuthenticationOptions identityOptionsCookies, SiteSettings siteSettings)
        {
            identityOptionsCookies.Cookie.Name = siteSettings.CookieOptions.CookieName;
            identityOptionsCookies.Cookie.HttpOnly = true;
            identityOptionsCookies.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            identityOptionsCookies.Cookie.SameSite = SameSiteMode.Lax;
            // this cookie will always be stored regardless of the user's consent
            identityOptionsCookies.Cookie.IsEssential = true;

            identityOptionsCookies.ExpireTimeSpan = siteSettings.CookieOptions.ExpireTimeSpan;
            identityOptionsCookies.SlidingExpiration = siteSettings.CookieOptions.SlidingExpiration;
            identityOptionsCookies.LoginPath = siteSettings.CookieOptions.LoginPath;
            identityOptionsCookies.LogoutPath = siteSettings.CookieOptions.LogoutPath;
            identityOptionsCookies.AccessDeniedPath = siteSettings.CookieOptions.AccessDeniedPath;
        }

        private static void SetLockoutOptions(LockoutOptions identityOptionsLockout, SiteSettings siteSettings)
        {
            identityOptionsLockout.AllowedForNewUsers = siteSettings.LockoutOptions.AllowedForNewUsers;
            identityOptionsLockout.DefaultLockoutTimeSpan = siteSettings.LockoutOptions.DefaultLockoutTimeSpan;
            identityOptionsLockout.MaxFailedAccessAttempts = siteSettings.LockoutOptions.MaxFailedAccessAttempts;
        }

        private static void SetPasswordOptions(PasswordOptions identityOptionsPassword, SiteSettings siteSettings)
        {
            identityOptionsPassword.RequireDigit = siteSettings.PasswordOptions.RequireDigit;
            identityOptionsPassword.RequireLowercase = siteSettings.PasswordOptions.RequireLowercase;
            identityOptionsPassword.RequireNonAlphanumeric = siteSettings.PasswordOptions.RequireNonAlphanumeric;
            identityOptionsPassword.RequireUppercase = siteSettings.PasswordOptions.RequireUppercase;
            identityOptionsPassword.RequiredLength = siteSettings.PasswordOptions.RequiredLength;
        }

        private static void SetSignInOptions(SignInOptions identityOptionsSignIn, SiteSettings siteSettings) => identityOptionsSignIn.RequireConfirmedEmail = siteSettings.EnableEmailConfirmation;

        private static void SetUserOptions(UserOptions identityOptionsUser) => identityOptionsUser.RequireUniqueEmail = true;

        #endregion Identity Options
    }
}
