using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Model
{
    public class ProductCategory : BaseEntity
    {
       // public string Id { get; set; } inherits Id from BaseEntity
        public string Name { get; set; }

        //public ProductCategory() { 
        //    this.Id = Guid.NewGuid().ToString();
        //}
    }
}
