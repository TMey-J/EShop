using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories;

public class ProvinceRepository(SQLDbContext context):GenericRepository<Province>(context),IProvinceRepository
{
    private readonly DbSet<Province>_provinces=context.Set<Province>();
    public async Task CreateAllAsync(List<Province> provinces)
    {
        await _provinces.AddRangeAsync(provinces);
    }
}