using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(OrderMetadata))]
    public partial class Order:IBaseEntity
    {
    }
    public class OrderMetadata
    {
        [Required]
        public string OrderNo { get; set; }
        [Required]
        public string CustomerOrganizationName { get; set; }
        [Required]
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        [Required]
        public string ContactPersonName { get; set; }
        [Required]
        public string ContactNo { get; set; }
        public Nullable<int> ProductType { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public string Quantity { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<double> PricePerUnit { get; set; }
        public Nullable<double> TotalPrice { get; set; }
        public Nullable<double> GrandTotalWithTax { get; set; }
        public Nullable<double> Advance { get; set; }
        public string Remarks { get; set; }
       
    }
}