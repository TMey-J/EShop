using EShop.Domain;

namespace EShop.Infrastructure.Configurations;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasOne(orderDetail => orderDetail.SellerProduct).WithMany(orderDetail => orderDetail.OrderDetails)
            .HasForeignKey(od => new { od.ProductId, od.SellerId, od.ColorId });
    }
}