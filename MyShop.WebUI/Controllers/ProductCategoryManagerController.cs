using MyShop.Core.Contracts;
using MyShop.Core.Model;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        //generic cache service 
        //InMemoryRepository<ProductCategory> context;

        //switching to interface so that it can use SQL/InMemory
        IRepository<ProductCategory> context;

        //public ProductCategoryManagerController()
        //{
        //    //create cache when called 
        //    context = new InMemoryRepository<ProductCategory>();
        //}

        //updating constructor to take in inteface as param, gets input from unity.config
        public ProductCategoryManagerController(IRepository<ProductCategory> catagoryContext)
        {
            context = catagoryContext;
        }
        public ActionResult Index()
        {
            List<ProductCategory> productsCategory = context.GetAll().ToList();
            return View(productsCategory);
        }
        //Display the page 
        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }
        //send the data
        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            else
            {
                context.Insert(productCategory);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult EditProductCategory(string Id)
        {
            ProductCategory productCategory = context.Find(Id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategory);
            }
        }

        [HttpPost]
        public ActionResult EditProductCategory(ProductCategory productCategory, string Id)
        {
            ProductCategory productCategoryToEdit = context.Find(Id);
            if (productCategoryToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productCategory);
                }
                else
                {
                    productCategoryToEdit.Name = productCategory.Name;
                    context.Commit();
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult DeleteProductCategory(string Id)
        {
            ProductCategory productCategoryToDelete = context.Find(Id);
            if (productCategoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategoryToDelete);
            }
        }

        [HttpPost]
        [ActionName("DeleteProductCategory")]
        //or overide the action name  @using (Html.BeginForm("ConfirmDelete","ProductManagerController"))
        public ActionResult ConfirmDelete(string Id)
        {
            ProductCategory productCategoryToDelete = context.Find(Id);

            if (productCategoryToDelete == null)
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