namespace EShop.Application.Contracts;

public interface ICityRepository:IGenericRepository<City>
{
    Task CreateAllAsync(List<City> provinces);
}