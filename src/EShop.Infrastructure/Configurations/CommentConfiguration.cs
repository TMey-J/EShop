namespace EShop.Infrastructure.Configurations;

public class CommentConfiguration:IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasOne(category => category.ParentComment).
            WithMany(category=>category.Replies)
            .HasForeignKey(category => category.ParentId);
    }
}