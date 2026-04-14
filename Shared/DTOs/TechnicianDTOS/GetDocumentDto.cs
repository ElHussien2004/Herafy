using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.TechnicianDTOS
{
    public class GetDocumentDto
    {
        public string? FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string? ProfileImageURL { get; set; }
        public string ServiceCategory { get; set; }
        public int ExperienceYears { get; set; }
        public string City { get; set; }

        public string Government { get; set; }
        public string? FaceImageUrl { get; set; }

        public string? BackImageUrl { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
