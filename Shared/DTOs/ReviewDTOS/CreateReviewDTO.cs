using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ReviewDTOS
{
    public class CreateReviewDTO
    {
        public int OrderId { get; set; }
        public int Rating { get; set; }//1--5

        public string? Comment { get; set; }
    }
}
