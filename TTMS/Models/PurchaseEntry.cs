//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TTMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PurchaseEntry
    {
        public int ID { get; set; }
        public Nullable<int> PurchaseOrderID { get; set; }
        public string InvoiceChallan { get; set; }
        public string InvoiceChallanNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<int> BrandID { get; set; }
        public string ProductName { get; set; }
        public string ProductColor { get; set; }
        public string ProductType { get; set; }
        public int ReceivedQuantity { get; set; }
        public double Price { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
        public double TotalAmount { get; set; }
        public Nullable<double> PaidAmount { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentRefNo { get; set; }
        public Nullable<double> DueAmount { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}