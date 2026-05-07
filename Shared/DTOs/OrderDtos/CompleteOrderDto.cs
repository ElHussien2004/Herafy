using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OrderDtos
{
    public class CompleteOrderDto
    {
        public IFormFile WorkImage { get; set; }
    }
}
