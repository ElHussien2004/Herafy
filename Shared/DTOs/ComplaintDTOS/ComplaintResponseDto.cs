using Domain.Entities.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ComplaintDTOS
{
    public class ComplaintResponseDto
    {
        public int ComplaintId { get; set; }
        public string Response { get; set; }
        public string Status { get; set; }
    }
}
