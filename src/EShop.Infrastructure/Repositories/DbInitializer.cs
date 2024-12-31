using Blogger.Application.Common.Exceptions;
using DNTCommon.Web.Core;
using EShop.Application.Common.Exceptions;
using EShop.Application.Constants;
using EShop.Application.Contracts.Identity;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Contracts.Services;
using EShop.Application.Model;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EShop.Infrastructure.Repositories;

public class DbInitializer(
    IServiceScopeFactory serviceFactory,
    IApplicationUserManager userManager,
    IApplicationRoleManager roleManager,
    IOptionsSnapshot<SiteSettings> siteSettings,
    IProvinceRepository province,
    ICityRepository city,
    IColorRepository color,
    IMongoColorRepository mongoColor,
    IMongoUserRepository mongoUserRepository,
    IMongoSellerRepository mongoSellerRepository,
    IMongoProvinceRepository mongoProvince,
    IMongoCityRepository mongoCity,
    IRabbitmqPublisherService publisher) : IDbInitializer
{
    private readonly IServiceScopeFactory _serviceFactory = serviceFactory;
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IApplicationRoleManager _roleManager = roleManager;
    private readonly IProvinceRepository _province = province;
    private readonly IMongoProvinceRepository _mongoProvince = mongoProvince;
    private readonly ICityRepository _city = city;
    private readonly IColorRepository _color = color;
    private readonly IMongoColorRepository _mongoColor = mongoColor;
    private readonly IMongoUserRepository _mongoUserRepository = mongoUserRepository;
    private readonly IMongoSellerRepository _mongoSellerRepository = mongoSellerRepository;
    private readonly IMongoCityRepository _mongoCity = mongoCity;
    private readonly IRabbitmqPublisherService _publisher = publisher;
    private readonly SiteSettings _siteSettings = siteSettings.Value;

    public void Initialize()
    {
        _serviceFactory.RunScopedService<SQLDbContext>(context => { context.Database.Migrate(); });
    }

    public void SeedData()
    {
        _serviceFactory.RunScopedService<IDbInitializer>(dbInitializer =>
        {
            dbInitializer.SeedRole(RoleNames.Admin).GetAwaiter().GetResult();
            dbInitializer.SeedRole(RoleNames.User).GetAwaiter().GetResult();
            dbInitializer.SeedRole(RoleNames.Seller).GetAwaiter().GetResult();
            dbInitializer.SeedRole(RoleNames.SystemSeller).GetAwaiter().GetResult();
            dbInitializer.SeedAdmin(_siteSettings.AdminUser).GetAwaiter().GetResult();
            dbInitializer.SeedProvinces().GetAwaiter().GetResult();
            dbInitializer.SeedCities().GetAwaiter().GetResult();
            dbInitializer.SeedSystemSeller(_siteSettings.SystemSeller).GetAwaiter().GetResult();
            dbInitializer.SeedColors().GetAwaiter().GetResult();
        });
    }

    public async Task SeedAdmin(AdminUser adminUser)
    {
        var user = await _userManager.FindByNameAsync(adminUser.UserName);
        if (user is not null)
        {
            return;
        }

        var adminRole = await _roleManager.FindByNameAsync(RoleNames.Admin);
        if (adminRole is null)
        {
            throw new CustomInternalServerException(["Admin role not found"]);
        }

        var isEmail = adminUser.EmailOrPhoneNumber.IsEmail();
        user = new User
        {
            UserName = adminUser.UserName,
            Email = isEmail ? adminUser.EmailOrPhoneNumber : null,
            PhoneNumber = isEmail ? null : adminUser.EmailOrPhoneNumber,
            PasswordHash = adminUser.Password.HashPassword(out var salt),
            PasswordSalt = salt,
            Avatar = _siteSettings.DefaultUserAvatar,
            IsActive = true,
            EmailConfirmed = isEmail,
            PhoneNumberConfirmed = !isEmail,
        };

        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new CustomBadRequestException(result.GetErrors());
        }

        var addToRulesResult = await _userManager.AddToRoleAsync(user, adminRole.Name!);
        if (!addToRulesResult.Succeeded)
        {
            throw new CustomBadRequestException(addToRulesResult.GetErrors());
        }
        user.UserRoles = null;
        user.UserClaims = null;
        user.UserTokens = null;
        user.UserLogins = null;
        await _publisher.PublishMessageAsync<User>(new(ActionTypes.Create, user), 
            RabbitmqConstants.QueueNames.User,
            RabbitmqConstants.RoutingKeys.User);
    }

    public async Task SeedRole(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role is not null)
        {
            return;
        }

        role = new Role
        {
            Name = roleName
        };
        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
            throw new CustomBadRequestException(result.GetErrors());
        }
    }

    public async Task SeedProvinces()
    {
        if (!await _province.IsAnyAsync())
        {
            var provincesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProvincesAndCities",
                "Provinces.json");
            var provincesDto =
                JsonConvert.DeserializeObject<List<ProvinceDto>>(await File.ReadAllTextAsync(provincesPath));
            if (provincesDto is null)
            {
                throw new CustomInternalServerException(["provinces file is not found"]);
            }

            var provinces = provincesDto.Select(x => new Province
            {
                Title = x.Name,
                Id = x.Id
            }).ToList();
            await _province.CreateAllAsync(provinces);
            await _province.SaveChangesAsync();

            // add provinces to mongodb
            var mongoProvinces = provincesDto.Select(x => new MongoProvince
            {
                Id = x.Id,
                Title = x.Name,
            }).ToList();
            await _mongoProvince.CreateAllAsync(mongoProvinces);
        }
    }

    public async Task SeedCities()
    {
        if (!await _city.IsAnyAsync())
        {
            var citiesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProvincesAndCities",
                "Cities.json");
            var citiesDto = JsonConvert.DeserializeObject<List<CityDto>>(await File.ReadAllTextAsync(citiesPath));
            if (citiesDto is null)
            {
                throw new CustomInternalServerException(["cities file is not found"]);
            }

            var cities = citiesDto.Select(x => new City
            {
                Id = x.Id,
                Title = x.Name,
                ProvinceId = x.Province_Id
            }).ToList();
            await _city.CreateAllAsync(cities);
            await _city.SaveChangesAsync();
            
            var mongoCities = citiesDto.Select(x => new MongoCity
            {
                Id = x.Id,
                Title = x.Name,
                ProvinceId = x.Province_Id
            }).ToList();
            await _mongoCity.CreateAllAsync(mongoCities);
        }
    }

    public async Task SeedSystemSeller(SystemSeller systemSeller)
    {
        var user = await _userManager.FindByNameAsync(systemSeller.UserName);
        if (user is not null)
        {
            return;
        }

        var systemSellerRole = await _roleManager.FindByNameAsync(RoleNames.SystemSeller);
        if (systemSellerRole is null)
        {
            throw new CustomInternalServerException(["Admin role not found"]);
        }

        var city = await _city.FindByAsync(nameof(City.Title), systemSeller.CityName)
                   ?? throw new CustomInternalServerException(["City not found"]);
        city.Province = await _province.FindByIdAsync(city.ProvinceId);
        var isEmail = systemSeller.EmailOrPhoneNumber.IsEmail();
        user = new User {
            UserName = systemSeller.UserName,
            Email = isEmail ? systemSeller.EmailOrPhoneNumber : null,
            PhoneNumber = isEmail ? null : systemSeller.EmailOrPhoneNumber,
            PasswordHash = systemSeller.Password.HashPassword(out var salt),
            PasswordSalt = salt,
            Avatar = _siteSettings.DefaultUserAvatar,
            IsActive = true,
            EmailConfirmed = isEmail,
            PhoneNumberConfirmed = !isEmail,
            SendCodeLastTime = DateTime.Now
        };
        var seller = new Seller{
            ShopName = systemSeller.ShopName,
            Address = systemSeller.Address,
            DocumentStatus = DocumentStatus.Confirmed,
            PostalCode = systemSeller.PostalCode,
            CityId = city.Id,
            City = city,
            UserName = user.UserName,
            IndividualSeller = new IndividualSeller()
            {
                NationalId = systemSeller.NationalId,
                CartOrShebaNumber = systemSeller.NationalId
            }
        };
        user.Seller = seller;

        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new CustomBadRequestException(result.GetErrors());
        }

        var addToRulesResult = await _userManager.AddToRoleAsync(user, systemSellerRole.Name!);
        if (!addToRulesResult.Succeeded)
        {
            throw new CustomBadRequestException(addToRulesResult.GetErrors());
        }

        var mongoUser = new MongoUser{
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            PasswordHash = user.PasswordHash,
            PasswordSalt = user.PasswordSalt,
            Avatar = user.Avatar,
            IsActive = true,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            Id = user.Id,
            NormalizedUserName = user.NormalizedUserName,
            NormalizedEmail = user.NormalizedEmail,
            ConcurrencyStamp = user.ConcurrencyStamp,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd,
            SecurityStamp = user.SecurityStamp,
            AccessFailedCount = user.AccessFailedCount,
            TwoFactorEnabled = user.TwoFactorEnabled,
            SendCodeLastTime = user.SendCodeLastTime
        };
        await _publisher.PublishMessageAsync<MongoUser>(new(ActionTypes.Create, mongoUser), 
            RabbitmqConstants.QueueNames.User,
            RabbitmqConstants.RoutingKeys.User);
        var mongoIndividualSeller= new MongoIndividualSeller(){
            NationalId = seller.IndividualSeller.NationalId,
            CartOrShebaNumber = seller.IndividualSeller.CartOrShebaNumber,
            AboutSeller = seller.IndividualSeller.AboutSeller
        };
        var mongoCity = new MongoCity() {
            Id = city.Id,
            ProvinceId = city.ProvinceId,
            Title = city.Title,
            IsDelete = city.IsDelete,
            Province = new MongoProvince
            {
                Id   = city.Province!.Id,
                Title = city.Province!.Title
            }
        };
        var mongoSeller = new MongoSeller() {
            Id = seller.Id,
            UserId = seller.UserId,
            UserName = seller.UserName,
            IsLegalPerson = seller.IsLegalPerson,
            Address = seller.Address,
            CityId = seller.CityId,
            City = mongoCity,
            Website = seller.Website,
            IsActive = true,
            DocumentStatus = DocumentStatus.Confirmed,
            PostalCode = seller.PostalCode,
            ShopName = seller.ShopName,
            Logo = seller.Logo,
            RejectReason = seller.RejectReason,
            IndividualSeller = mongoIndividualSeller
        };
        await _publisher.PublishMessageAsync<MongoSeller>(new(ActionTypes.Create, mongoSeller), 
            RabbitmqConstants.QueueNames.Seller,
            RabbitmqConstants.RoutingKeys.Seller);
    }

    public async Task SeedColors()
    {
        if (await _color.IsAnyAsync())
        {
           return; 
        }
        var colorsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
            "colors.json");
        var colorsDto = JsonConvert.DeserializeObject<Dictionary<string,string>>(await File.ReadAllTextAsync(colorsFilePath));
        if (colorsDto is null)
        {
            throw new CustomInternalServerException(["colors file is not found"]);
        }

        var colors = colorsDto.Select(x =>
            new Color { ColorName = x.Key.Trim(), ColorCode = x.Value.Trim() }).ToList();
        await _color.CreateAllAsync(colors);
        await _color.SaveChangesAsync();
        var mongoColors = colors.Select(x =>
            new MongoColor { ColorName = x.ColorName, ColorCode = x.ColorCode,Id = x.Id}).ToList();
        await _mongoColor.CreateAllAsync(mongoColors);
        
    }
}