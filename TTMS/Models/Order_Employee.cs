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
    
    public partial class Order_Employee
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public System.DateTime Date { get; set; }
        public string Field { get; set; }
        public Nullable<int> NoOfWorksPerDay { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public int OrderID { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Order Order { get; set; }
    }
}
