using MyShop.Core.Contracts;
using MyShop.Core.Model;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> catagoryContext;
        
        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            catagoryContext = productCategoryContext;
        }
        public ActionResult Index(string category = null)
        {
            List<Product> products;
            List<ProductCategory> categories = catagoryContext.GetAll().ToList();

            if(category == null)
            {
                products =context.GetAll().ToList();
            }
            else
            {
                products = context.GetAll().Where(p => p.Category == category).ToList();
            }
            ProductListViewModel model = new ProductListViewModel();
            model.Products = products;
            model.ProductCategories = categories;
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Details(string Id) {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }

        }
    }
}