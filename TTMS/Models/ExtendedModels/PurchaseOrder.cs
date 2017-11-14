﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    [MetadataType(typeof(PurchaseOrderMetadata))]
    public partial class PurchaseOrder:IBaseEntity
    {
    }
    public class PurchaseOrderMetadata
    {
       
        [RegularExpression("([0-9]+)")]
        public string PurchaseOrderNo { get; set; }
     
        public int SupplierID { get; set; }
    }
}