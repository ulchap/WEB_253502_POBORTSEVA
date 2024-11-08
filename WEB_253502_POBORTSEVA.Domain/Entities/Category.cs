﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253502_POBORTSEVA.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public List<Product> Products { get; set; }

        public Category()
        { 
            Products = new List<Product>();
        }
    }
}
