using EShop.Domain.Entities;
using EShop.Domain.Entities.Identity;
using EShop.Infrastucture.Extentions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EShop.Infrastucture.Databases
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
