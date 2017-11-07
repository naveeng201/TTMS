using System;
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
        [Required]
        public string PurchaseOrderNo { get; set; }
        [Required]
        public int SupplierID { get; set; }
    }
}