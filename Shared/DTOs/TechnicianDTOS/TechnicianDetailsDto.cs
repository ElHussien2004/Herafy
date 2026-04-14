using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.TechnicianDTOS
{
    public class TechniciaDetailsDto
    {
        public string Fullname { get; set; }
        public string? ProfileImageURL { get; set; }
        public string ServiceCategory { get; set; }
        public int ExperienceYears { get; set; }
        public double RatingAvg { get; set; }
        public bool AvailabilityStatus { get; set; }//هو متاح للطلب ولا لا 
        public decimal InspectedPrice { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set;}
        public bool IsActive { get; set; }
        public string City { get; set; }

        public string Government { get; set; }
    }
}
