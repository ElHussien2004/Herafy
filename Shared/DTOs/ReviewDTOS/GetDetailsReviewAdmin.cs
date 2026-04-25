using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ReviewDTOS
{
    public class GetDetailsReviewAdmin
    {
        public int Id { get; set; }
        public string NameClient { get; set; }

        public string NameTechnician { get; set; }
        public int Rating { get; set; }//1--5

        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool is_suspicious { get; set; } // AI هيرجع true لو التقييم مشبوه
        public float ConfidenceScore { get; set; } // نسبة تأكد الـ AI من قراره
        public string? FraudReasons { get; set; }
    }
}
