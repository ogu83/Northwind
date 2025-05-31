using System.Linq.Expressions;

namespace NorthwindApi.Providers;

public static class IQueryableExtensions
{
    public static IQueryable<T> Contains<T>(this IQueryable<T> source, string propertyName, string value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var constant = Expression.Constant(value);
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })
            ?? throw new InvalidOperationException($"The Contains method was not found on the string type.");
        var containsCall = Expression.Call(property, containsMethod, constant);
        var lambda = Expression.Lambda<Func<T, bool>>(containsCall, parameter);
        return source.Where(lambda);
    }

    public static IQueryable<T> Where<T>(this IQueryable<T> source, string propertyName, object value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var constant = Expression.Constant(value);
        var equality = Expression.Equal(property, constant);
        var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);
        return source.Where(lambda);
    }

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        => source.OrderBy(ToLambda<T>(propertyName));

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        => source.OrderByDescending(ToLambda<T>(propertyName));

    private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
}