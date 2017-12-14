using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class CustomerVM
    {
        public Customer customer { get; set; }
        public CustomerAddress customerAddress { get; set; }
        public Address address { get; set; }
    }
}