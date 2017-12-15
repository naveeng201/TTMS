using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class Order_MasterVM
    {
        public Order_Master order_Master { get; set; }
        public OrderItems orderItems { get; set; }
        public List<OrderMasterProducts> order_Master_products { get; set; }
    }
    public class OrderMasterProducts
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public int RemainingQuantity { get; set; }
    }
}