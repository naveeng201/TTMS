using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class CartProduct
    {
        public SaleProduct prodduct { get; set; } 
        public int quantity { get; set; }
    }
}