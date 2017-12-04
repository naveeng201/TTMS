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
        [Display(Name = "Order No")]
        public string OrderNo { get; set; }
        [Required]
        [Display(Name = "Customer Organization Name")]
        public string CustomerOrganizationName { get; set; }
        [Required]
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        [Required]
        [Display(Name = "Contact Person Name")]
        public string ContactPersonName { get; set; }
        [Required]
        [Display(Name = "Contact Number")]
        public string ContactNo { get; set; }
        [Display(Name = "Product Type")]
        public Nullable<int> ProductType { get; set; }
        
        public string Size { get; set; }
        
        public string Quantity { get; set; }
        [Display(Name = "Delivery Date")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        [Display(Name = "Price Per Unit")]
        public Nullable<double> PricePerUnit { get; set; }
        [Display(Name = "Total Price")]
        public Nullable<double> TotalPrice { get; set; }
        [Display(Name = "Grand Total With Tax")]
        public Nullable<double> GrandTotalWithTax { get; set; }
        public Nullable<double> Advance { get; set; }
        public string Remarks { get; set; }
       
    }
}