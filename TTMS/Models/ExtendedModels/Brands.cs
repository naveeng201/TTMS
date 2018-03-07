using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(BrandsMasterMetaData))]
    public partial class brandsmaster : IBaseEntity
    {

    }

    public class BrandsMasterMetaData
    {
        [Required]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }
    }
}