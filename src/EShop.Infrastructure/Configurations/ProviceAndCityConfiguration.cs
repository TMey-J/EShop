using EShop.Domain;

namespace EShop.Infrastructure.Configurations;

public class ProvinceConfiguration:IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
public class CityConfiguration:IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}