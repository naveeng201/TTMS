using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public class CartProduct
    {
        public int productID { get; set; }
        public double price { get; set; }
        public string name { get; set; }
        public string imagePath { get; set; }
        public int quantity { get; set; }
    }
}