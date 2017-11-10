using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class PurcheseOrderVM
    {
        public PurchaseOrder purchaseOrder { get; set; }
        public OrderDetail _orderDetail { get; set; } 
    }
}