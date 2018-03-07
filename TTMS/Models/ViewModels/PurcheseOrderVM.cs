using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class PurchaseOrderVM
    {
       
        public purchaseorder purchaseOrder { get; set; }
        public List<orderdetail> _orderDetail { get; set; } 
        
    }
}