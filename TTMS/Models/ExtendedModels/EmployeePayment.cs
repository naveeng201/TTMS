using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(EmployeePaymentMetadata))]
    public partial class EmployeePayment:IBaseEntity
    {
    }
    public class EmployeePaymentMetadata
    {
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public int PaymentID { get; set; }
        [Required]
        public System.DateTime PaymentDate { get; set; }
        [Required]
        public double Amount { get; set; }
       
    }
}