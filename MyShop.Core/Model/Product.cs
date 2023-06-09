﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Model
{
    public class Product : BaseEntity
    {
        
       // public string Id { get; set; } inherits Id from BaseEntity
        [StringLength(20)]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, 1000)]
        public decimal Price { get; set; }
        public string Category { get; set; }

        public string Image { get; set; }
        
        //under references add caching in the InMemory solution
        //system.runtime.caching
        //Add reference to solution that has the model
    }
}
