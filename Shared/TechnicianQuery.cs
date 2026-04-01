using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class TechnicianQuery
    {
        public int? ServiceId { get; set; }
        public TecnicianSorting Sorting { get; set; }
        public double? ClientLatitude { get; set; }
        public double? ClientLongitude { get; set; }
    }
}
