using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class OrderVM
    {
        public Order order { get; set; }
        public IEnumerable<OrderItems> orderItems { get; set; }
        public Customer customer { get; set; }
        public CustomerAddress customerAddress { get; set; }
    }
}