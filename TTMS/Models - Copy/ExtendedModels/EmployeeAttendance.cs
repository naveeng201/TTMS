using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(EmployeeAttendanceMetadata))]
    public partial class EmployeeAttendance
    {
    }

    public class EmployeeAttendanceMetadata
    {
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public System.TimeSpan StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public string Remarks { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}