using EShop.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EShop.Infrastructure.Databases
{
    public class SQLDbContext(DbContextOptions options) :
        IdentityDbContext<User,Role,long,UserClaim,UserRole,UserLogin,RoleClaim,UserToken>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(SQLDbContext).Assembly);
            builder.RegisterAllEntities(typeof(BaseEntity));
            builder.AddIsDeleteQueryFilter<BaseEntity>();
        }
    }
}
