using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(PurchaseEntryMetadata))]
    public partial class PurchaseEntry:IBaseEntity
    {
    }
    public class PurchaseEntryMetadata
    {
        public Nullable<int> PurchaseOrderID { get; set; }
        public string InvoiceChallan { get; set; }
        public string InvoiceChallanNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<int> BrandID { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductColor { get; set; }
        [Required]
        public string ProductType { get; set; }
        [Required]
        public int ReceivedQuantity { get; set; }
        [Required]
        public double Price { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
        [Required]
        public double TotalAmount { get; set; }
        public Nullable<double> PaidAmount { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentRefNo { get; set; }
        public Nullable<double> DueAmount { get; set; }
    }
}