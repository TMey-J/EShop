using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace EShop.Infrastructure.Extensions;

public static class EfExtensions
{
    public static void RegisterAllEntities(this ModelBuilder builder, Type type)
    {
        var entities = type.Assembly.GetTypes().Where(x => x.BaseType == type);
        foreach (var entity in entities)
            builder.Entity(entity);
    }

    public static void AddIsDeleteQueryFilter<TBaseEntity>(this ModelBuilder builder) where TBaseEntity : BaseEntity
    {
        Expression<Func<TBaseEntity, bool>> filterExpr = bm => !bm.IsDelete;
        foreach (var mutableEntityType in builder.Model.GetEntityTypes())
        {
            if (mutableEntityType.ClrType.IsAssignableTo(typeof(TBaseEntity)))
            {
                var parameter = Expression.Parameter(mutableEntityType.ClrType);
                var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                var lambdaExpression = Expression.Lambda(body, parameter);

                mutableEntityType.SetQueryFilter(lambdaExpression);
            }
        }
    }
}