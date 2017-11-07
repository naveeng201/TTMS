using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models 
{
    [MetadataType(typeof(SuppliersMetadata))]
    public partial class Supplier : IBaseEntity
    {
       
    }
    public class SuppliersMetadata
    {
        public int ID { get; set; }
        [Required]
        public string OrganizationName { get; set; }
        [Required]
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pin { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public string GSTNo { get; set; }
    }
}