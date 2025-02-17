namespace EShop.Infrastructure.Configurations;

public class SellerProductConfiguration:IEntityTypeConfiguration<SellerProduct>
{
    public void Configure(EntityTypeBuilder<SellerProduct> builder)
    {
        builder.HasOne(sellerProduct => sellerProduct.Product).
            WithMany(product=>product.SellersProducts)
            .HasForeignKey(sellerProduct => sellerProduct.ProductId);

        builder.HasOne(sellerProduct => sellerProduct.Seller).
            WithMany(seller => seller.SellersProducts)
            .HasForeignKey(sellerProduct => sellerProduct.SellerId);
        
        builder.HasOne(sellerProduct => sellerProduct.Color).
            WithMany(color => color.SellerProducts)
            .HasForeignKey(sellerProduct => sellerProduct.ColorId);

        builder.HasKey(key => new { key.ProductId,key.SellerId,key.ColorId });

        builder.ToTable("SellerProducts");
    }
}