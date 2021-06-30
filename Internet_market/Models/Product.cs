using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internet_market.Models
{
    public class Product
    {
        public int id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string Material { get; set; }

        

        public List <string> Photos;
    }
}
