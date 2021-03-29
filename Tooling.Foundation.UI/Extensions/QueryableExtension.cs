using System.Linq;

namespace Tooling.Foundation.Extensions
{
    public static class QueryableExtension
    {
        public static IOrderedQueryable<TSource> Order<TSource, TKey>(
            this IQueryable<TSource> source,
            System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector, bool ascending)
        {
            if (ascending)
            {
                return source.OrderBy(keySelector);
            }
            return source.OrderByDescending(keySelector);
        }
    }
}