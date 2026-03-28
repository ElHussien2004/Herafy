using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ClientDTOS
{
    public class UpdateClientDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string ProfileImageURL { get; set; }
        public string City { get; set; }
        public string Government { get; set; }
    }
}
