namespace EShop.Application.Contracts
{
    public interface IColorRepository:IGenericRepository<Color>
    {
        Task CreateAllAsync(List<Color> colors);

    }
}
