using Domain.Entities.UsersEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Communications
{
    public class Notification : BaseEntity<int>
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public NotificationType Type { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
