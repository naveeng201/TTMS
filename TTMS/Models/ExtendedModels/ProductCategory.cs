using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(ProductCategoryMetadata))]
    public partial class productcategory : IBaseEntity
    {
    }
    public  class ProductCategoryMetadata
    {
        [Required]
        public string Name { get; set; }
    }
     

}