using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(BrandsMasterMetaData))]
    public partial class BrandsMaster : IBaseEntity
    {

    }

    public class BrandsMasterMetaData
    {
        [Required]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }
    }
}