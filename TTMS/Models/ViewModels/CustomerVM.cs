using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class CustomerVM
    {
        public customer customer { get; set; }
        public customeraddress customerAddress { get; set; }
        public address address { get; set; }
    }
}