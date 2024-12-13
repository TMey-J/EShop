namespace EShop.Infrastructure.Configurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.HasOne(roleClaim => roleClaim.Role)
            .WithMany(role => role.RoleClaims)
            .HasForeignKey(roleClaim => roleClaim.RoleId);

        builder.ToTable("RoleClaims");
    }
}