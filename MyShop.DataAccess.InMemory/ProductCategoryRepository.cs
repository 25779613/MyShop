using MyShop.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;


namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productsCategory;

        public ProductCategoryRepository()
        {
            productsCategory = cache["productCategory"] as List<ProductCategory>;

            if (productsCategory == null)
            {
                productsCategory = new List<ProductCategory>();

               
            }
        }

        public void Commit()
        {
            cache["productCategory"] = productsCategory;
        }

        public void Insert(ProductCategory category)
        {
            if (String.IsNullOrEmpty(category.Id))
            {
                category.Id = Guid.NewGuid().ToString();
            }
            productsCategory.Add(category);
        }

        public void Update(ProductCategory category)
        {
            //find the product in the list
            ProductCategory productcategoryToUpdate = productsCategory.Find(p => p.Id == category.Id);

            if (productcategoryToUpdate != null)
            {
                productcategoryToUpdate = category;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        public ProductCategory Find(string Id)
        {
            ProductCategory category = productsCategory.Find(p => p.Id == Id);

            if (category != null)
            {
                return category;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        //return list of productsCategory
        public IQueryable<ProductCategory> GetAllproductsCategory()
        {
            return productsCategory.AsQueryable();
        }

        public void Delete(string Id)
        {
            ProductCategory productcategoryToDelete = productsCategory.Find(p => p.Id == Id);

            if (productcategoryToDelete != null)
            {
                productsCategory.Remove(productcategoryToDelete);
            }
            else
            {
                throw new Exception("Product not found");
            }
        }
    }
}
