using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(UnitMetadata))]
    public partial class unit:IBaseEntity
    {
    }
    public class UnitMetadata
    {
       
        [Required]
        public string Name { get; set; }
    }
}