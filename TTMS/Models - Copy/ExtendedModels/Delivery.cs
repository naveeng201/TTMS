using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(DeliveryMetadata))]
    public partial class Delivery: IBaseEntity
    {
    }
    public class DeliveryMetadata
    {
        public Nullable<int> OrderID { get; set; }
        [Required]
        public int EmployeeID { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string ChallanNo { get; set; }
        public string DeliveredProducts { get; set; }
        public string BalanceOrder { get; set; }
    }
}