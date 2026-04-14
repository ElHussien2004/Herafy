using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ClientDTOS
{
    public class UpdataClientdto
    {
        public string FullName { get; set; }
        public IFormFile ImageUrl { get; set; }
        public string City { get; set; }

        public string Government { get; set; }
    }
}
