﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253502_POBORTSEVA.Domain.Entities
{
    public class CartItem
    {
        public Product? Product { get; set; }
        public int Count { get; set; }
    }
}
