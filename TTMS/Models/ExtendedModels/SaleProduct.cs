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
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public Nullable<double> Price { get; set; }
        public string Description { get; set; }
        public Nullable<int> ProductCategoryID { get; set; }
        public Nullable<int> BrandID { get; set; }
        public Nullable<int> UnitID { get; set; }
    }
}