using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTMS.Models
{
    public interface IBaseEntity
    {
        [ScaffoldColumn(false)]
        Nullable<System.DateTime> CreatedDate { get; set; }
        [ScaffoldColumn(false)]
        Nullable<int> CreatedBy { get; set; }
        [ScaffoldColumn(false)]
        Nullable<System.DateTime> ModifiedDate { get; set; }
        [ScaffoldColumn(false)]
        Nullable<int> ModifiedBy { get; set; }
    }
}
