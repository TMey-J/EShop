using EShop.Domain;

namespace EShop.Infrastructure.Configurations;

public class CategoryFeatureConfiguration:IEntityTypeConfiguration<CategoryFeature>
{
    public void Configure(EntityTypeBuilder<CategoryFeature> builder)
    {
        builder.HasOne(categoryFeature => categoryFeature.Category).
            WithMany(category=>category.CategoryFeatures)
            .HasForeignKey(categoryFeature => categoryFeature.CategoryId);

        builder.HasOne(categoryFeature => categoryFeature.Feature).
            WithMany(feature => feature.CategoryFeatures)
            .HasForeignKey(categoryFeature => categoryFeature.FeatureId);

        builder.HasKey(key => new { key.CategoryId,key.FeatureId });

        builder.ToTable("CategoryFeatures");
    }
}