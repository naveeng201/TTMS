using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(PurchaseEntryMetadata))]
    public partial class purchaseentry:IBaseEntity
    {
    }
    public class PurchaseEntryMetadata
    {
        public Nullable<int> PurchaseOrderID { get; set; }
        [Required]
        public string InvoiceChallan { get; set; }
        [Required]
        public string InvoiceChallanNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
        public Nullable<double> DiscountAmount { get; set; }
        public double TotalAmount { get; set; }
        public Nullable<double> DueAmount { get; set; }
        public bool Status { get; set; }
    }
}