using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class OrderVM
    {
        public order order { get; set; }
        public List<orderitem> orderItems { get; set; }
        public customer customer { get; set; }
        public address address { get; set; }
    }
}