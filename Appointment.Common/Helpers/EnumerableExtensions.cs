using System;
using System.Linq;
using System.Linq.Expressions;

namespace Appointment.Common.Helpers
{
    public static class EnumerableExtensions
    {
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, string property, Expression<Func<TSource, bool>> predicate)
        {
            if (!string.IsNullOrWhiteSpace(property))
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, int? property, Expression<Func<TSource, bool>> predicate)
        {
            if (property.HasValue)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, bool property, Expression<Func<TSource, bool>> predicate)
        {
            if (property)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, DateTime? property, Expression<Func<TSource, bool>> predicate)
        {
            if (property.HasValue)
            {
                source = source.Where(predicate);
            }

            return source;
        }
    }
}
