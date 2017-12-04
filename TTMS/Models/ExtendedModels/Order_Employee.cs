using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(Order_EmployeeMetadata))]
    public partial class Order_Employee:IBaseEntity
    {
    }
    public class Order_EmployeeMetadata
    {
        [Required]
        public int OrderID { get; set; }
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public System.DateTime Date { get; set; }
        [Required]
        public string Field { get; set; }
        public Nullable<int> NoOfWorksPerDay { get; set; }
    }
}