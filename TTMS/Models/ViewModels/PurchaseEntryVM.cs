using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class PurchaseEntryVM
    {
        public purchaseorder purchaseOrder { get; set; }
        public purchaseentry purchaseEntry { get; set; }
        public List<orderdetail> _orderDetail { get; set; }

    }
}