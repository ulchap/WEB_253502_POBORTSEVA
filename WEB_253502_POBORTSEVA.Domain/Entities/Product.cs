using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253502_POBORTSEVA.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category? Category { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public string? MimeType { get; set; }

    }
}
