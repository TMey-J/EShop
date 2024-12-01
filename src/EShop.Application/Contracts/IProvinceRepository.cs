namespace EShop.Application.Contracts;

public interface IProvinceRepository:IGenericRepository<Province>
{
    Task CreateAllAsync(List<Province> provinces);
}