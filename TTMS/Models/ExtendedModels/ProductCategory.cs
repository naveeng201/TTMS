using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(ProductCategoryMetadata))]
    public partial class ProductCategory : IBaseEntity
    {
    }
    public  class ProductCategoryMetadata
    {
        public string Name { get; set; }
    }
     

}