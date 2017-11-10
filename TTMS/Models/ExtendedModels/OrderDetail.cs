using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(OrderDetailMetadata))]
    public partial class OrderDetail:IBaseEntity
    {
    }
    public class OrderDetailMetadata
    {
        [Required]
        public int PurchaseOrderID { get; set; }
        [Required]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductColor { get; set; }
        public string ProductType { get; set; }
        public string Quantity { get; set; }
        public string TypeOfUnits { get; set; }
    }
}