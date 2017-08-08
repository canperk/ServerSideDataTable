using DataTableServerSide.Context;
using DataTableServerSide.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataTableServerSide.Helpers
{
    public class CacheHelper
    {
        private static object _syncLock = new object();
        public static List<SelectItem> GetCategories(IMemoryCache cache, NrthContext context)
        {
            var key = nameof(Category);
            if (cache.TryGetValue(key, out object values))
            {
                var items = values as List<SelectItem>;
                return items;
            }
            else
            {
                lock (_syncLock)
                {
                    if (cache.TryGetValue(key, out object value))
                        return value as List<SelectItem>;

                    var items = context.Categories.Select(p => new SelectItem(p.CategoryId.ToString(), p.CategoryName)).ToList();
                    cache.Set(key, items);
                    return items;
                }
            }
        }
        public static List<SelectItem> GetCompanies(IMemoryCache cache, NrthContext context)
        {
            var key = nameof(Supplier);
            if (cache.TryGetValue(key, out object values))
            {
                var items = values as List<SelectItem>;
                return items;
            }
            else
            {
                lock (_syncLock)
                {
                    if (cache.TryGetValue(key, out object value))
                        return value as List<SelectItem>;

                    var items = context.Suppliers.Select(p => new SelectItem(p.SupplierId.ToString(), p.CompanyName)).ToList();
                    cache.Set(key, items);
                    return items;
                }
            }
        }

        public static List<SelectItem> GetProducts(IMemoryCache cache, NrthContext context)
        {
            var key = nameof(Product);
            if (cache.TryGetValue(key, out object values))
            {
                var items = values as List<SelectItem>;
                return items;
            }
            else
            {
                lock (_syncLock)
                {
                    if (cache.TryGetValue(key, out object value))
                        return value as List<SelectItem>;

                    var items = context.Products.Select(p => new SelectItem(p.ProductId.ToString(), p.ProductName)).ToList();
                    cache.Set(key, items);
                    return items;
                }
            }
        }
    }
}
