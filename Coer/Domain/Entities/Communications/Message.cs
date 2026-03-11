
using Domain.Entities.UsersEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Communications
{
    public class Message : BaseEntity<int>
    {
        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }

        public string Content { get; set; }

        public MessageType Type { get; set; }

        public string ImageUrl { get; set; }

        public bool IsRead { get; set; }

        public DateTime SentAt { get; set; }
    }
}
