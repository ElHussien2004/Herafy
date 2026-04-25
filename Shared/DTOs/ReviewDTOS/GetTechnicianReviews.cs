using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ReviewDTOS
{
    public class GetTechnicianReviews
    {
        public int Id { get; set; }
        public int Rating { get; set; }//1--5

        public string Comment { get; set; }

        public string NameClient { get; set; }
        public string ImageURLClient { get; set; }
    }
}
