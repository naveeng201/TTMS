using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class OrderVM
    {
        public Order order { get; set; }
        public List<OrderItem> orderItems { get; set; }
        public Customer customer { get; set; }
        public Address address { get; set; }
    }
}