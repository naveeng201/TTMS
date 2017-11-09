using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product:IBaseEntity
    {
    }
    public class ProductMetadata
    {
        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Display(Name = "Product Short Name")]
        public string ShorName { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public string Color { get; set; }
        public string Type { get; set; }
        public Nullable<int> BrandID { get; set; }
        public Nullable<int> ProductCategoryID { get; set; }
        public Nullable<int> Price { get; set; }
    }
}