using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DataTableServerSide.Helpers
{
    public static class IQueryableExtensions
    {
        public static IEnumerable<T> OrderBy<T>(this IQueryable<T> query, string name, DTOrderDir direction)
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var sortDir = direction == DTOrderDir.ASC ? "OrderBy" : "OrderByDescending";
            var method = typeof(Enumerable).GetMethods().FirstOrDefault(m => m.Name == sortDir && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() });
        }

        public static SelectResult GetSelectItems(this IQueryable<SelectItem> query, SelectRequest request)
        {
            IEnumerable<SelectItem> items = null;
            if (request.Values.Any())
            {
                items = query.Where(i => request.Values.Contains(i.Id)).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(request.SearchTerm))
                    items = query.Where(i => i.Text.ToLower().StartsWith(request.SearchTerm.ToLower())).ToList();
                else
                    items = query.ToList();
            }
            var count = query.Count();
            var queryResult = items.Take(request.PageSize).ToList();
            return new SelectResult { Total = count, Results = queryResult };
        }

        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            var expr = Expression.Lambda(propAccess, paramExpr);
            return expr;
        }

        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            var properties = objType.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name == name);
            if (matchedProperty == null)
                throw new ArgumentException("name");

            return matchedProperty;
        }

        public static object ToCollectionResult<T>(this IQueryable<T> query, DTParameters param)
        {
            var filteredCount = query.Count();
            var sortColumn = param.Order.Any() ? param.Columns[param.Order.First().Column].Name : param.Columns.First().Name;
            var sortDir = param.Order.Any() ? param.Order.First().Dir : DTOrderDir.ASC;

            var data = query.OrderBy(sortColumn, sortDir)
                              .Skip(param.Start)
                              .Take(param.Length).ToList();
            return new DTResult<T>
            {
                Draw = param.Draw,
                Data = data,
                RecordsFiltered = filteredCount,
            };
        }
    }
}
