using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DataTableServerSide.Context;
using Microsoft.EntityFrameworkCore;
using DataTableServerSide.ViewModels;
using DataTableServerSide.Helpers;

namespace DataTableServerSide.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetProducts([FromBody]DTParameters param)
        {
            var ctx = new NrthContext();
            var query = ctx.Products.Include(i => i.Category).Include(i => i.Supplier).Where(i => i.UnitsInStock > 0);
            var filteredCount = query.Count();
            var sortColumn = param.Order.Any() ? param.Columns[param.Order.First().Column].Name : param.Columns.First().Name;
            var sortDir = param.Order.Any() ? param.Order.First().Dir : DTOrderDir.ASC;

            var products = query.OrderBy(i => i.ProductId)
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
                RecordsFiltered = filteredCount,
            };

            return Json(result);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
