using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ClientDTOS
{
    public class AddClientDto
    {
        
        public string FullName { get; set; }
        public IFormFile? ProfileImageURL { get; set; }
        public string City { get; set; }
        public string Government { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}
