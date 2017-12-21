using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TTMS.Models
{
    [MetadataType(typeof(SaleProductMetadata))]
    public partial class SaleProduct
    {
    }
    public class SaleProductMetadata
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Type { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public string Color { get; set; }
        public Nullable<double> Price { get; set; }
        public string Description { get; set; }
        [Display(Name ="Product Category")]
        public Nullable<int> ProductCategoryID { get; set; }
        [Display(Name = "Name")]
        public Nullable<int> BrandID { get; set; }
        [Display(Name = "Unit")]
        public Nullable<int> UnitID { get; set; }
    }
}