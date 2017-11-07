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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string ContactNo { get; set; }
        public string AlternateContactNo { get; set; }
        public Nullable<int> Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Designation { get; set; }
        public Nullable<bool> MasterEmp { get; set; }
        public string ExperiencedField { get; set; }
        public string Experience { get; set; }
        public Nullable<int> Salary { get; set; }
        public Nullable<int> NoLeavesPerMonth { get; set; }
    }
      
}