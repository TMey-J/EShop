using EShop.Infrastructure.Databases;

namespace EShop.Infrastructure.Repositories;

public class FeatureRepository(SQLDbContext context) : GenericRepository<Feature>(context), IFeatureRepository
{
}