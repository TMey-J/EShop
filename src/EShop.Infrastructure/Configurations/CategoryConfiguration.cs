using EShop.Domain;

namespace EShop.Infrastructure.Configurations;

public class CategoryConfiguration:IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasOne(category => category.Parent).
            WithMany(category=>category.Categories)
            .HasForeignKey(category => category.ParentId);
    }
}