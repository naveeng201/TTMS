using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class PurchaseEntryVM
    {
        public PurchaseOrder purchaseOrder { get; set; }
        public PurchaseEntry purchaseEntry { get; set; }
        public List<OrderDetail> _orderDetail { get; set; }

    }
}