using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.TechnicianDTOS
{
    public class TechnicianDto
    {
        public string UserId { get; set; }

        public int ServiceCategoryId { get; set; }

        public int ExperienceYears { get; set; }

        public bool AvailabilityStatus { get; set; }

        public double RatingAvg { get; set; }

        public string Bio { get; set; }

        public decimal InspectedPrice { get; set; }

        public int CompletedJobs { get; set; }

        public bool IsActive { get; set; }

        public string FaceImageUrl { get; set; }

        public string BackImageUrl { get; set; }
    }
}
