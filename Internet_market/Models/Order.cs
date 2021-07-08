using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internet_market.Models
{
    public class Order
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime dateTime { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
