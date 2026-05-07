using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ComplaintDTOS
{
    public class CreateComplaintDto
    {
        public int? OrderId { get; set; }
        public string? Title { get; set; }
        public string Description { get; set; }
    }
}
