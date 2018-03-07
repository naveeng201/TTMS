using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class OrdersVM
    {
        public Order order { set; get; }
        public List<OrderItem> orderItems { set; get; }
    }
}