using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internet_market.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string source { get; set; }
       
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
