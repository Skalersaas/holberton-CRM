using System.Linq.Expressions;
using System.Reflection;

namespace Utilities.Services
{
    public static class QueryMaster<T>
    {
        public static PropertyInfo GetProperty(string fieldName)
        {
            return typeof(T).GetProperty(fieldName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                ?? throw new ArgumentNullException(fieldName);

        }
        public static IQueryable<T> OrderByField(IQueryable<T> source, string fieldName, bool ascending)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, fieldName);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = ascending ? "OrderBy" : "OrderByDescending";
            var result = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type)
                .Invoke(null, [source, lambda]);

            return (IQueryable<T>)result!;
        }
        public static IQueryable<T> FilterByFields(IQueryable<T> source, Dictionary<string, string> filters)
        {
            if (filters == null || filters.Count == 0)
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? combinedExpression = null;

            var stringType = typeof(string);
            var objectType = typeof(T);
            var toStringMethod = typeof(object).GetMethod("ToString")!;
            var containsMethod = stringType.GetMethod("Contains", [stringType])!;
            foreach (var filter in filters)
            {
                var property = objectType.GetProperty(filter.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property == null)
                    continue;

                Expression propertyExpression = Expression.Property(parameter, property);

                if (property.PropertyType != stringType)
                {
                    propertyExpression = Expression.Call(propertyExpression, toStringMethod);
                }

                var constant = Expression.Constant(filter.Value);

                Expression condition = Expression.Call(propertyExpression, containsMethod, constant);

                combinedExpression = combinedExpression == null
                    ? condition
                    : Expression.AndAlso(combinedExpression, condition);
            }

            if (combinedExpression == null)
                return source;

            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            Console.WriteLine(lambda.ToString());
            return source.Where(lambda);
        }
    }
}
