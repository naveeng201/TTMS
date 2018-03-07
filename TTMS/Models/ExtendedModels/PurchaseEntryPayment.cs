using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(PurchaseEntryPaymentMetadata))]
    public partial class purchaseentrypayment : IBaseEntity
    {
    }
    public class PurchaseEntryPaymentMetadata
    {
        [Display(Name ="Payment Date")]
        public System.DateTime PaymentDate { get; set; }
        [Display(Name = "Payment Mode")]
        public string PaymentMode { get; set; }
        [Display(Name = "Payment Reference No.")]
        public string PaymentRefNo { get; set; }
        [Display(Name = "Payment Amount")]
        public Nullable<double> PaidAmount { get; set; }
    }
}