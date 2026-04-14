using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.UserDTOS
{
    public class AuthResultDto
    {
        public string UserId { get; set; }
        public bool IsAuthenticated { get; set; }

        public string Token { get; set; }

        public DateTime ExpiresOn { get; set; }

        public string PhoneNumber { get; set; }
        public bool IsNew { get; set; }
    }
}
