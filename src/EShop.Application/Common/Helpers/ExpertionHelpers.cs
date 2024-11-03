using System.Linq.Expressions;

namespace Marketplace.Common.Helpers;
public static class ExperssionHelpers
{
    public static Expression<Func<T,bool>> CreateAnyExperssion<T>(string propertyName,object propertyValue)
    {
        var parameter= Expression.Parameter(typeof(T));
        var property=Expression.Property(parameter, propertyName);
        if (propertyValue is string)
        {
            propertyValue=propertyValue.ToString()!.Trim();
        }
        var constantValue=Expression.Constant(propertyValue);
        var equal=Expression.Equal(property, constantValue);
        return Expression.Lambda<Func<T, bool>>(equal, parameter);
       
    }
    public static Expression<Func<T,bool>> CreateFindByExperssion<T>(string propertyName, object propertyValue)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        if (propertyValue is string)
        {
            propertyValue = propertyValue.ToString()!.Trim();
        }
        var constantValue = Expression.Constant(propertyValue);
        var equal = Expression.Equal(property, constantValue);
        return Expression.Lambda<Func<T, bool>>(equal, parameter);

    }
    //public static IOrderedQueryable<T> CreateOrderByExperssion<T>(this IQueryable<T> query,string propertyName,string sortintAs)
    //{
    //    var parameter = Expression.Parameter(typeof(T));
    //    var conversion = Expression.Convert(Expression.Property(parameter, propertyName),typeof(object));
    //    var exp= Expression.Lambda<Func<T, object>>(conversion, parameter);
    //    IOrderedQueryable<T> result = sortintAs == "Asc" ? query.OrderBy(exp) : query.OrderByDescending(exp);
    //    return result;

    //}
}
