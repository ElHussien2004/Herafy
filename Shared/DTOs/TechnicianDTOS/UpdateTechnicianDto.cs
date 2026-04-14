using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.TechnicianDTOS
{
    public class UpdateTechnicianDto
    {
        
        public string FullName { get; set; }
        public IFormFile ImageUrl { get; set; }
        public string Bio { get; set; }
        public int ExperienceYears { get; set; }

        public decimal InspectedPrice { get; set; }

        public string City { get; set; }

        public string Government { get; set; }

    }
}
