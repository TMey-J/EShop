using EShop.Application.Configs.MediatR;
using EShop.Application.Constants;
using EShop.Application.Contracts.Services;
using EShop.Application.Model;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace EShop.Infrastucture
{
    public static class DependencyInjection
    {
        private const string EmailConfirmationTokenProviderName = "ConfirmEmail";

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SiteSettings>(configuration.Bind);
            var siteSettings = configuration.Get<SiteSettings>(configuration.Bind);
            ArgumentNullException.ThrowIfNull(siteSettings);

            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
            services.AddJwtAuthentication(siteSettings.JwtConfigs);
            services.AddAuthorizationPolicies();

            return services;
        }
       
        private static void AddJwtAuthentication(this IServiceCollection services, JwtConfigs jwtConfigs)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigs.SecretKey));
                options.SaveToken = true;
                options.TokenValidationParameters = new()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfigs.Issuer,
                    ValidAudience = jwtConfigs.Audience,
                    IssuerSigningKey = securityKey
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;

                        var result = "ابتدا وارد اکانت خود شوید";

                        await context.Response.WriteAsJsonAsync(result);

                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;

                        var result = "اجازه ورود به این صفحه را ندارید";

                        await context.Response.WriteAsJsonAsync(result);

                    }
                };
            });
        }
        private static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy(PolicyNames.Admin, policy => policy.RequireRole(RoleNames.Admin))
                .AddPolicy(PolicyNames.User, policy => policy.RequireRole(RoleNames.User));
        }
    }

}

