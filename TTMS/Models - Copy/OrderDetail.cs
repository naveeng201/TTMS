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
    
    public partial class orderdetail
    {
        public int ID { get; set; }
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
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual product product { get; set; }
        public virtual purchaseorder purchaseorder { get; set; }
        public virtual unit unit { get; set; }
    }
}