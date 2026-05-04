using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ChatDTOS
{
    public class ChatSummaryDto
    {
        public int ChatId { get; set; }
        public string OtherPartyName { get; set; } // اسم الشخص التاني (فني أو عميل)
        public string OtherPartyPicture { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageAt { get; set; }
        public int UnreadCount { get; set; } // عدد الرسايل اللي ملقرأتش
    }
}
