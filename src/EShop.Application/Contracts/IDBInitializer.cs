namespace EShop.Application.Contracts;

public interface IDbInitializer
{
    void Initialize();
    void SeedData();
    Task SeedAdmin(AdminUser adminUser);
    Task SeedRole(string roleName);
    Task SeedProvinces();
    Task SeedCities();
}