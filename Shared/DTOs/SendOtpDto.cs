using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
     public class SendOtpDto
     {
        [Required]
        public string PhoneNumber { get; set; }
        public UserType UserType { get; set; }
    }
}
