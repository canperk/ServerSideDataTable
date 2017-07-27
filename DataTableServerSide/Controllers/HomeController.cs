using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DataTableServerSide.Context;
using Microsoft.EntityFrameworkCore;
using DataTableServerSide.ViewModels;
using DataTableServerSide.Helpers;
using DataTableServerSide.Entities;

namespace DataTableServerSide.Controllers
{
    public class HomeController : Controller
    {
        private NrthContext _ctx;
        public HomeController()
        {
            _ctx = new NrthContext();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Select()
        {
            return View();
        }
        public IActionResult GetProducts([FromBody]DTParameters param)
        {
            var query = _ctx.Products.Include(i => i.Category).Include(i => i.Supplier).Where(i => i.UnitsInStock > 0).Select(i => new ProductViewModel
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
        public IActionResult SaveProduct([FromBody]ProductViewModel model)
        {
            Product product;
            if (model.IsNew)
            {
                product = new Product
                {
                    ProductName = model.Name,
                    CategoryId = model.CategoryId,
                    UnitsInStock = model.Stock,
                    UnitPrice = model.Price
                };
                _ctx.Products.Add(product);
            }
            else
            {
                product = _ctx.Products.FirstOrDefault(i => i.ProductId == model.Id);
                if (product == null)
                    return Json(false);
                product.ProductName = model.Name;
                product.CategoryId = model.CategoryId;
                product.UnitsInStock = model.Stock;
                product.UnitPrice = model.Price;
            }
            var result = _ctx.SaveChanges() > 0;
            return Json(result);
        }
    }
}
