using Domain.Entities.Communications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class UnreadMessagesSpecification:BaseSpecifications<Message>
    {
        public UnreadMessagesSpecification(int chatId, string currentUserId)
        :base(m => m.ChatId == chatId &&
                    m.IsRead == false &&
                    m.SenderId != currentUserId)
        {
            AddInclude(m => m.Sender);
        }
    }
}
