using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class PurchaseOrderVM
    {
       
        public PurchaseOrder purchaseOrder { get; set; }
        public List<OrderDetail> _orderDetail { get; set; } 
        
    }
}