using System.Linq.Expressions;

namespace EShop.Application.Common.Helpers;

public static class ExpressionHelpers
{
    public static Expression<Func<T, bool>> CreateAnyExpression<T>(string propertyName, object propertyValue)
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

    public static Expression<Func<T, bool>> CreateFindByExpression<T>(string propertyName, object propertyValue)
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

    public static IQueryable<T> CreateContainsExpression<T>(this IQueryable<T> query, string propertyName,
        string propertyValue)
    {
        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            return query;
        }

        var parameterExp = Expression.Parameter(typeof(T));
        Expression propertyExp = parameterExp;
        if (propertyName.Contains('.'))
        {
            foreach (var member in propertyName.Split('.'))
            {
                propertyExp = Expression.PropertyOrField(propertyExp, member);
            }
        }
        else
        {
            propertyExp = Expression.Property(parameterExp, propertyName);
        }
        var method = typeof(string).GetMethod("Contains", [typeof(string)])!;
        var someValue = Expression.Constant(propertyValue, typeof(string));
        var containsMethodExp = Expression.Call(propertyExp, method, someValue);
        var exp = Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);
        return query.Where(exp);
    }

    public static IOrderedQueryable<T> CreateOrderByExpression<T>(
        this IQueryable<T> query,
        string propertyName,
        SortingAs sortingAs)
    {
        var parameterExp = Expression.Parameter(typeof(T));
        Expression propertyExp = parameterExp;
        if (propertyName.Contains('_'))
        {
            propertyName = propertyName.Replace('_', '.');
            foreach (var member in propertyName.Split('.'))
            {
                propertyExp = Expression.PropertyOrField(propertyExp, member);
            }
        }
        else
        {
            propertyExp = Expression.Property(parameterExp, propertyName);
        }

        var conversion = Expression.Convert(propertyExp, typeof(object));
        var exp = Expression.Lambda<Func<T, object>>(conversion, parameterExp);
        var result =
            sortingAs == SortingAs.Ascending ? query.OrderBy(exp) : query.OrderByDescending(exp);
        return result;
    }

    public static IQueryable<T> CreateDeleteStatusExpression<T>(
        this IQueryable<T> query,
        string propertyName,
        DeleteStatus deleteStatus)
    {
        var parameter = Expression.Parameter(typeof(T));
        var conversion = Expression.Property(parameter, propertyName);
        var exp = Expression.Lambda<Func<T, bool>>(conversion, parameter);
        var result = deleteStatus switch
        {
            DeleteStatus.False => query.Where(exp.Not()),
            DeleteStatus.OnlyDeleted => query.Where(exp),
            _ => query
        };
        return result;
    }

    private static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> exp)
    {
        var candidateExpr = exp.Parameters[0];
        var body = Expression.Not(exp.Body);

        return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
    }
}