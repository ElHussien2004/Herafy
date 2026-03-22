using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.TechnicianDTOS
{
    public class UpdateTechnicianDto
    {
        public string Id { get; set; }
        public int ServiceCategoryId { get; set; }
        public int ExperienceYears { get; set; }
        public string Bio { get; set; }
        public decimal InspectedPrice { get; set; }
        public bool AvailabilityStatus { get; set; }
    }
}
