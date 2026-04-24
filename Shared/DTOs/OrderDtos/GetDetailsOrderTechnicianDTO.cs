using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OrderDtos
{
    public class GetDetailsOrderTechnicianDTO
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public DateTime ScheduledDate { get; set; }
        public TimeSpan ScheduledTime { get; set; }
        public string PlaceDetails { get; set; }
        public string ProblemDetails { get; set; }
        public decimal InspectedPrice { get; set; }
        public decimal AfterPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public string ImageCliURL { get; set; }
        public string NameClient { get; set; }
        
        public string State { get; set; }
    }
}
