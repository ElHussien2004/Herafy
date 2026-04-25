using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ReviewDTOS
{
    public class FraudCheckRequestDto
    {
        public float rating_value { get; set; }
        public int text_length { get; set; }
        public float rating_deviation { get; set; }
        public int is_night_owl { get; set; }
    }
    public class FraudCheckResponseDto
    {
        public bool is_suspicious { get;set; }
        public float confidence_score { get; set; }
        public string? FraudReasons { get; set; }
    }
}
