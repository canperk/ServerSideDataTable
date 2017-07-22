using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataTableServerSide.Context;
using Microsoft.EntityFrameworkCore;
using DataTableServerSide.ViewModels;
using Newtonsoft.Json;
using DataTableServerSide.Helpers;

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
}
