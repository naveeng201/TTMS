using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(OrderMetadata))]
    public partial class order:IBaseEntity
    {
    }
    public class OrderMetadata
    {
        
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<double> TotalPrice { get; set; }
        public Nullable<double> GST { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> GrandTotalWithTax { get; set; }
        public Nullable<double> Advance { get; set; }
        public Nullable<int> Status { get; set; }
        public string Remarks { get; set; }
    }
}