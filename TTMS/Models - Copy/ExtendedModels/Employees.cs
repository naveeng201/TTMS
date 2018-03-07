using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee : IBaseEntity
    {

    }

    public class EmployeeMetadata
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Contact No")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string ContactNo { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        [Display(Name = "Alternate Contact No")]
        public string AlternateContactNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Designation { get; set; }
        [Display(Name = "Master Employee")]
        public Nullable<bool> MasterEmp { get; set; }
        public string ExperiencedField { get; set; }
        public string Experience { get; set; }
        public Nullable<int> Salary { get; set; }
        [Display(Name = "No. of Leaves Per Month")]
        public Nullable<int> NoLeavesPerMonth { get; set; }
    }
      
}