using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DataTableServerSide.Context;
using Microsoft.EntityFrameworkCore;
using DataTableServerSide.ViewModels;
using DataTableServerSide.Helpers;
using DataTableServerSide.Entities;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;

namespace DataTableServerSide.Controllers
{
    public partial class HomeController : Controller
    {
        private NrthContext _ctx;
        private readonly IMemoryCache _cache;
        public HomeController(IMemoryCache cache)
        {
            _ctx = new NrthContext();
            _cache = cache;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Select()
        {
            return View();
        }
        public IActionResult MultipleItem()
        {
            return View();
        }
        public IActionResult GetCompanies([FromBody]DTParameters param)
        {
            var query = _ctx.Suppliers.Include(i => i.Products).Select(i => new SupplierViewModel
            {
                Id = i.SupplierId,
                Name = i.CompanyName,
                Country = i.Country,
                City = i.City,
                Manager = i.ContactName,
                Products = i.Products.Select(a => a.ProductId.ToString()),
                FormattedProducts = string.Join(", ", i.Products.Select(a => a.ProductName).ToList()) 
            });
            return Json(query.ToCollectionResult(param));
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
                CompanyId = i.SupplierId.Value,
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
                    UnitPrice = model.Price,
                    SupplierId = model.CompanyId
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
                product.SupplierId = model.CompanyId;
            }
            var result = _ctx.SaveChanges() > 0;
            return Json(result);
        }
        public IActionResult GetCategories(SelectRequest request)
        {
            var categories = CacheHelper.GetCategories(_cache, _ctx).AsQueryable();
            var result = categories.GetSelectItems(request);
            return Json(result);
        }
        public IActionResult GetSuppliers(SelectRequest request)
        {
            var companies = CacheHelper.GetCompanies(_cache, _ctx).AsQueryable();
            var result = companies.GetSelectItems(request);
            return Json(result);
        }

        public IActionResult GetProductsAutoComplete(SelectRequest request)
        {
            var companies = CacheHelper.GetProducts(_cache, _ctx).AsQueryable();
            var result = companies.GetSelectItems(request);
            return Json(result);
        }
        public IActionResult GetProductsByCategoryId(int id)
        {
            var p = _ctx.Products.Where(i => i.CategoryId == id).Select(i => new SelectItem(i.ProductId.ToString(), i.ProductName)).ToList();
            return Json(p);
        }
    }
}
