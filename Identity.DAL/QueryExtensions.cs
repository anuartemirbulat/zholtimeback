using System.Linq.Expressions;

namespace Identity.DAL;

public static class QueryExtensions
{
    public static void Delete<T>(this IQueryable<T> queryable, T entity) where T : BaseEntity
    {
        entity.IsDeleted = true;
    }

    public static int MaxOrDefault<T>(this IQueryable<T> source, Expression<Func<T, int?>> selector,
        int nullValue = 0)
    {
        return source.Max(selector) ?? nullValue;
    }
}
