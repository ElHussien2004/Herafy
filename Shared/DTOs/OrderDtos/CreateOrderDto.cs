using Domain.Entities.OrderEntity;
using Domain.Entities.UsersEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OrderDtos
{
    public class CreateOrderDto
    {
        
        public string ClientId { get; set; } 
        public string TechnicianId { get; set; }
        public int ServiceId { get; set; } 

        public string City { get; set; }

        public string Government { get; set; }

        public string PlaceDetails { get; set; }
        public string ProblemDetails { get; set; }
        public DateTime ScheduledDate { get; set; }

        public TimeSpan ScheduledTime { get; set; }

        public decimal InspectedPrice { get; set; }
    }
}
