using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OrderDtos
{
    public class GetClientOrderDTO
    {
        public int Id {  get; set; }
        public string ServiceName {  get; set; }
        public DateTime ScheduledDate { get; set; }

        public TimeSpan ScheduledTime { get; set; }
        public decimal InspectedPrice { get; set; }

        public string State { get; set; }
    }
}
