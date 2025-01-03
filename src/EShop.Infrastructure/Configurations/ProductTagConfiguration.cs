namespace EShop.Infrastructure.Configurations;

public class ProductTagConfiguration:IEntityTypeConfiguration<ProductTag>
{
    public void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        builder.HasOne(productTag => productTag.Product).
            WithMany(product=>product.ProductTags)
            .HasForeignKey(productTag => productTag.ProductId);

        builder.HasOne(productTag => productTag.Tag).
            WithMany(tag => tag.ProductTags)
            .HasForeignKey(productTag => productTag.TagId);

        builder.HasKey(key => new { key.TagId,key.ProductId });

        builder.ToTable("productTags");
    }
}