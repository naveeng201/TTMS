﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class Order_MasterVM
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int EmployeeID { get; set; }
        public string OrderNo { get; set; }
        public string EmployeeName { get; set; }
        public string  Status { get; set; }
        public List<OrderMasterProducts> order_Master_products { get; set; }
    }
    public class OrderMasterProducts
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public int RemainingQuantity { get; set; }
    }
}