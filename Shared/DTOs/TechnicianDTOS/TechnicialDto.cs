using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.TechnicianDTOS
{
    public class TechnicialDto
    {
       
        public string Fullname { get; set; }
        public string? ProfileImageURL { get; set; }
        public string ServiceCategory { get; set; }
        public int ExperienceYears { get; set; }
        public string Bio { get; set; }
        public bool AvailabilityStatus { get; set; }//هو متاح للطلب ولا لا 
        public bool IsActive { get; set; }
        public double RatingAvg { get; set; }

        public int CompletedJobs { get; set; }

    }
}
