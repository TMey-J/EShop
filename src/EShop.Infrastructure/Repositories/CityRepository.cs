using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories;

public class CityRepository(SQLDbContext context):GenericRepository<City>(context),ICityRepository
{
    private readonly DbSet<City> _cities=context.Set<City>();
    public async Task CreateAllAsync(List<City> provinces)
    {
        await _cities.AddRangeAsync(provinces);
    }
}
