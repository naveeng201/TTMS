using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(OrderDetailMetadata))]
    public partial class orderdetail:IBaseEntity
    {
    }
    public class OrderDetailMetadata
    {
        public int PurchaseOrderID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductColor { get; set; }
        public string ProductType { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> ReceivedQuantity { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<double> CostPrice { get; set; }
        public Nullable<double> MRP { get; set; }
        public Nullable<double> SalePrice { get; set; }
    }
}