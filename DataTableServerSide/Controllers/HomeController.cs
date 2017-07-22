using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataTableServerSide.Context;
using Microsoft.EntityFrameworkCore;
using DataTableServerSide.ViewModels;
using Newtonsoft.Json;

namespace DataTableServerSide.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetProducts(DTParameters param)
        {
            var ctx = new NrthContext();
            var products = ctx.Products.Include(i => i.Category).Include(i => i.Supplier)
                              .Where(i => i.UnitsInStock > 0)
                              .OrderBy(i => i.ProductId)
                              .Skip(param.Start)
                              .Take(param.Length)
                              .Select(i => new ProductViewModel
                              {
                                  Id = i.ProductId,
                                  Name = i.ProductName,
                                  Price = i.UnitPrice.Value,
                                  Stock = i.UnitsInStock.Value,
                                  Category = i.Category.CategoryName,
                                  CategoryId = i.CategoryId.Value,
                                  Company = i.Supplier.CompanyName
                              }).ToList();
            var result = new DTResult<ProductViewModel>
            {
                Draw = param.Draw,
                Data = products,
                RecordsFiltered = 74,
                RecordsTotal = 77
            };

            return Json(result);
        }

        public IActionResult Error()
        {
            return View();
        }
    }

    public class DTResult<T>
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<T> Data { get; set; }
    }

    public abstract class DTRow
    {
        public virtual string DT_RowId
        {
            get { return null; }
        }

        public virtual string DT_RowClass { get { return null; } }

        public virtual object DT_RowData { get { return null; } }
    }

    public class DTParameters
    {
        public DTParameters()
        {
            Columns = new List<DTColumn>();
            Order = new List<DTOrder>();
        }
        public int Draw { get; set; }
        public List<DTColumn> Columns { get; set; }
        public List<DTOrder> Order { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public DTSearch Search { get; set; }
        public string SortOrder
        {
            get
            {
                return Columns != null && Order != null && Order.Count > 0
                    ? (Columns[Order[0].Column].Data + (Order[0].Dir == DTOrderDir.DESC ? " " + Order[0].Dir : string.Empty))
                    : null;
            }
        }
    }
    public class DTSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }
    public class DTColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }

    }
    public class DTOrder
    {
        public int Column { get; set; }
        public DTOrderDir Dir { get; set; }
    }
    public enum DTOrderDir
    {
        ASC,
        DESC
    }
}
