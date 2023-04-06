using MyShop.Core.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class DataContext : DbContext
    {
        // the create the connection and tables
        public DataContext() : base("DefaultConnection") { }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        //after adding tables use ef migration - Add-Migration Name & change the default project name to the file containing the data context
        // update the db - Update-Database
    }

   
}
