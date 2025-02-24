﻿namespace EShop.Infrastructure
{
    public static class DependencyInjection
    {
        private const string EmailConfirmationTokenProviderName = "ConfirmEmail";

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment environment)
        {
            var siteSettings = configuration.Get<SiteSettings>(configuration.Bind);
            ArgumentNullException.ThrowIfNull(siteSettings);

            services.AddSqlDataBase(siteSettings.ConnectionStrings.SQLDbContextConnection);
            services.AddSingleton<MongoDbContext>();
            services.AddIdentityServices();
            services.AddIdentityOptions(siteSettings);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IPrincipal>(provider =>
                provider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User ?? ClaimsPrincipal.Current!);
            services.ConfigureServices(environment);
            services.ConfigureSqlRepositories();
            services.ConfigureMongoRepositories();
            services.AddScoped<IDbInitializer, DbInitializer>();
            return services;
        }

        private static void AddSqlDataBase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<SQLDbContext>((options) =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        private static void ConfigureServices(this IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddScoped<IRabbitmqPublisherService, RabbitmqPublisherService>();
            if (environment.IsDevelopment())
            {
                services.AddScoped<ISmsSenderService, LocalSmsSenderService>();
                services.AddScoped<IEmailSenderService, LocalEmailSenderService>();
            }
            else
            {
                services.AddScoped<IEmailSenderService, EmailSenderService>();
                services.AddScoped<ISmsSenderService, KavenegarSmsSenderService>();
            }
        }

        private static void ConfigureSqlRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryFeatureRepository, CategoryFeatureRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IFeatureRepository, FeatureRepository>();
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IColorRepository, ColorRepository>();
            services.AddScoped<ISellerProductRepository, SellerProductRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
        }
        private static void ConfigureMongoRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMongoTagRepository, MongoTagRepository>();
            services.AddScoped<IMongoUserRepository, MongoUserRepository>();
            services.AddScoped<IMongoProvinceRepository, MongoProvinceRepository>();
            services.AddScoped<IMongoCityRepository, MongoCityRepository>();
            services.AddScoped<IMongoFeatureRepository, MongoFeatureRepository>();
            services.AddScoped<IMongoCategoryFeatureRepository, MongoCategoryFeatureRepository>();
            services.AddScoped<IMongoCategoryRepository, MongoCategoryRepository>();
            services.AddScoped<IMongoSellerRepository, MongoSellerRepository>();
            services.AddScoped<IMongoProductRepository, MongoProductRepository>();
            services.AddScoped<IMongoColorRepository, MongoColorRepository>();
            services.AddScoped<IMongoSellerProductRepository, MongoSellerProductRepository>();
            services.AddScoped<IMongoCommentRepository, MongoCommentRepository>();
            services.AddScoped<IMongoOrderRepository, MongoOrderRepository>();
            services.AddScoped<IMongoOrderDetailRepository, MongoOrderDetailRepository>();
        }
        private static void AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
            services.AddScoped<UserManager<User>, ApplicationUserManager>();

            services.AddScoped<IApplicationRoleManager, ApplicationRoleManager>();
            services.AddScoped<RoleManager<Role>, ApplicationRoleManager>();

            services.AddScoped<IApplicationSignInManager, ApplicationSignInManager>();
            services.AddScoped<SignInManager<User>, ApplicationSignInManager>();

            services.AddScoped<IJwtService, JwtService>();
        }

        private static void AddIdentityOptions(this IServiceCollection services, SiteSettings siteSettings)
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

            services.Replace(ServiceDescriptor.Scoped<IUserValidator<User>, CustomUserValidator<User>>());

            services.ConfigureApplicationCookie(identityOptionsCookies =>
            {
                SetApplicationCookieOptions(identityOptionsCookies, siteSettings);
            });

            services.EnableImmediateLogout();
        }

        public static void InitializeDb(this IServiceProvider serviceProvider)
        {
            serviceProvider.RunScopedService<IDbInitializer>(dbInitialize =>
            {
                dbInitialize.Initialize();
                dbInitialize.SeedData();
            });
        }

        #region Identity Options

        private static void AddConfirmEmailDataProtectorTokenOptions(this IServiceCollection services,
            SiteSettings siteSettings)
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
                options.OnRefreshingPrincipal =
                    new Func<SecurityStampRefreshingPrincipalContext, Task>(_ => Task.CompletedTask);
            });
        }

        private static void SetApplicationCookieOptions(CookieAuthenticationOptions identityOptionsCookies,
            SiteSettings siteSettings)
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

        private static void SetSignInOptions(SignInOptions identityOptionsSignIn, SiteSettings siteSettings)
            => identityOptionsSignIn.RequireConfirmedEmail = siteSettings.EnableEmailConfirmation;

        private static void SetUserOptions(UserOptions identityOptionsUser) =>
            identityOptionsUser.RequireUniqueEmail = true;

        #endregion Identity Options
    }
}