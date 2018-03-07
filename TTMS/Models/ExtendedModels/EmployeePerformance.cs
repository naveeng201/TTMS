using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(EmployeePerformanceMetadata))]
    public partial class employeeperformance:IBaseEntity
    {
    }
    public class EmployeePerformanceMetadata
    {
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public string JobType { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public System.DateTime Date { get; set; }
       
    }
}