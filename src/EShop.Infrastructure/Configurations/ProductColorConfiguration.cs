namespace EShop.Infrastructure.Configurations;

public class ProductColorConfiguration:IEntityTypeConfiguration<ProductColor>
{
    public void Configure(EntityTypeBuilder<ProductColor> builder)
    {
        builder.HasOne(productColor => productColor.Product).
            WithMany(product=>product.ProductColors)
            .HasForeignKey(productColor => productColor.ProductId);

        builder.HasOne(productColor => productColor.Color).
            WithMany(color => color.ProductColors)
            .HasForeignKey(productColor => productColor.ColorId);

        builder.HasKey(key => new { key.ColorId,key.ProductId });

        builder.ToTable("productColors");
    }
}