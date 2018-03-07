using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models 
{
    [MetadataType(typeof(SuppliersMetadata))]
    public partial class supplier : IBaseEntity
    {
       
    }
    public class SuppliersMetadata
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Organization Name")]
        public string OrganizationName { get; set; }
        [Required]
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pin { get; set; }
        [Display(Name = "Contact Person Name")]
        public string ContactPersonName { get; set; }
        [Display(Name = "Contact Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string ContactNo { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Invalid Email Address")]
        public string EmailId { get; set; }
        [Display(Name = "GST Number")]
        public string GSTNo { get; set; }
    }
}