using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(OrderItemsMetaData))]
    public partial class orderitems
    {
    }

    public class OrderItemsMetaData
    {
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> Price { get; set; }
    }
}