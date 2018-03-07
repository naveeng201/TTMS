using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(Order_MasterMetadata))]
    public partial class order_master:IBaseEntity
    {
    }
    public class Order_MasterMetadata
    {
         
        public int OrderID { get; set; }
        [Display(Name ="Employee")]
        public int EmployeeID { get; set; }
    }
}