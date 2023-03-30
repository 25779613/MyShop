using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//add using for caching and model
using System.Runtime.Caching;
using MyShop.Core.Model;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository() { 
            products = cache["product"] as List<Product>;

            if (products == null) {
               products = new List<Product>();
         
            Product sam = new Product();
            sam.Name = "sam";
            sam.Price = 020;
            sam.Description = "bla";
            sam.Category = "kaka";
            Insert(sam);
            Commit();
            }
        }

        public void Commit()
        {
            cache["product"] = products;
        }

        public void Insert(Product product)
        {
            if(String.IsNullOrEmpty(product.Id))
            {
                product.Id = Guid.NewGuid().ToString();
            }
            products.Add(product);
        }

        public void Update(Product product)
        {
            //find the product in the list
            Product productToUpdate = products.Find(p => p.Id == product.Id);

            if(productToUpdate != null)
            {
                productToUpdate = product;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        public Product Find(string Id)
        {
            Product product = products.Find(p => p.Id == Id);

            if (product != null)
            {
                return product; 
            }
            else
            {
                throw new Exception("Product not found");
            }
        }
        
        //return list of products
        public IQueryable<Product> GetAllProducts()
        {
            return products.AsQueryable();
        }

        public void Delete(string Id)
        {
            Product productToDelete = products.Find(p => p.Id == Id);

            if (productToDelete != null)
            {
                products.Remove(productToDelete);
            }
            else
            {
                throw new Exception("Product not found"); 
            }
        }
    }
}
