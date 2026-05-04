using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ChatDTOS
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
        public string Type { get; set; } // Text or Image
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
