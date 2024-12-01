using Blogger.Application.Common.Exceptions;
using DNTCommon.Web.Core;
using EShop.Application.Common.Exceptions;
using EShop.Application.Constants;
using EShop.Application.Contracts.Identity;
using EShop.Application.Model;
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
    ICityRepository city) : IDbInitializer
{
    private readonly IServiceScopeFactory _serviceFactory = serviceFactory;
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IApplicationRoleManager _roleManager = roleManager;
    private readonly IProvinceRepository _province = province;
    private readonly ICityRepository _city = city;
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
            dbInitializer.SeedAdmin(_siteSettings.AdminUser).GetAwaiter().GetResult();
            dbInitializer.SeedProvinces().GetAwaiter().GetResult();
            dbInitializer.SeedCities().GetAwaiter().GetResult();

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
            IsActive = true
        };
        if (user.Email is not null)
        {
            user.EmailConfirmed = true;
        }
        else
        {
            user.PhoneNumberConfirmed = true;
        }
        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new CustomBadRequestException(result.GetErrors());
        }
        var addToRulesResult= await _userManager.AddToRoleAsync(user, adminRole.Name!);
        if (!addToRulesResult.Succeeded)
        {
            throw new CustomBadRequestException(addToRulesResult.GetErrors());
        }
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
            var provincesPath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","ProvincesAndCities", "Provinces.json");
            var provincesDto = JsonConvert.DeserializeObject<List<ProvinceDto>>(await File.ReadAllTextAsync(provincesPath));
            if (provincesDto is null)
            {
                throw new CustomInternalServerException(["provinces file is not found"]);
            }
            var provinces = provincesDto.Select(x => new Province
            {
                Title = x.Name,
            }).ToList();
            await _province.CreateAllAsync(provinces);
            await _province.SaveChangesAsync();
        }
    }

    public async Task SeedCities()
    {
        if (!await _city.IsAnyAsync())
        {
            var citiesPath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","ProvincesAndCities", "Cities.json");
            var citiesDto = JsonConvert.DeserializeObject<List<CityDto>>(await File.ReadAllTextAsync(citiesPath));
            if (citiesDto is null)
            {
                throw new CustomInternalServerException(["cities file is not found"]);
            }
            var cities = citiesDto.Select(x => new City
            {
                Title = x.Name,
                ProvinceId = x.Province_Id
            }).ToList();
            await _city.CreateAllAsync(cities);
            await _city.SaveChangesAsync();
        }
    }
}