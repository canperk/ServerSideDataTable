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
            var query = ctx.Products.Include(i => i.Category).Include(i => i.Supplier).Where(i => i.UnitsInStock > 0).Select(i => new ProductViewModel
            {
                Id = i.ProductId,
                Name = i.ProductName,
                Price = i.UnitPrice.Value,
                Stock = i.UnitsInStock.Value,
                Category = i.Category.CategoryName,
                CategoryId = i.CategoryId.Value,
                Company = i.Supplier.CompanyName
            }); ;
            return Json(query.ToCollectionResult(param));
        }
        [HttpPost]
        public IActionResult SaveProduct(ProductViewModel model)
        {
            return Json(true);
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
