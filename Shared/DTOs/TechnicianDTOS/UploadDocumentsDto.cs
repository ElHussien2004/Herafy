using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.TechnicianDTOS
{
    public class UploadDocumentsDto
    {
        public IFormFile FaceImage { get; set; }
        public IFormFile BackImage { get; set; }
    }
}
