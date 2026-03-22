using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.TechnicianDTOS
{
    public class AddTechnicianDto
    {
        public string UserId { get; set; }
        public int ServiceCategoryId { get; set; }

        public int ExperienceYears { get; set; }

        public string Bio { get; set; }

        public decimal InspectedPrice { get; set; }
    }
}
