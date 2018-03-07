using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class OrdersVM
    {
        public order order { set; get; }
        public List<orderitem> orderItems { set; get; }
    }
}