using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Model;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        //cache service 
        InMemoryRepository<Product> context;
        InMemoryRepository<ProductCategory> catagoryContext;

        public ProductManagerController() {
            //create cache when called 
            context = new InMemoryRepository<Product>();
            catagoryContext = new InMemoryRepository<ProductCategory>();
        }
        public ActionResult Index()
        {
            List<Product> products = context.GetAll().ToList();
            return View(products);
        }
        //Display the page 
        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.Product= new Product();
            viewModel.ProductCategories = catagoryContext.GetAll();
            return View(viewModel);
        }
        //send the data
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult EditProduct(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = catagoryContext.GetAll();
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult EditProduct(Product product,string Id)
        {
            Product productToEdit = context.Find(Id);
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if(!ModelState.IsValid)
                {
                    return View(product);
                }
                else
                {
                    productToEdit.Category = product.Category;
                    productToEdit.Description = product.Description;
                    productToEdit.Image = product.Image;
                    productToEdit.Name = product.Name;
                    productToEdit.Price = product.Price;

                    context.Commit();
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult DeleteProduct( string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("DeleteProduct")]
        //or overide the action name  @using (Html.BeginForm("ConfirmDelete","ProductManagerController"))
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}