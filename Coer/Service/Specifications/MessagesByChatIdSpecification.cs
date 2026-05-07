using Domain.Entities.Communications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class MessagesByChatIdSpecification : BaseSpecifications<Message>
    {
        public MessagesByChatIdSpecification(int chatId, int skip, int take):base(m=>m.ChatId==chatId)
        {
            AddOrderByDescending(m => m.SentAt);
            AddInclude(m => m.Chat);
            // طبق البجنيشن (هات كم رسالة وسيب كام)
            ApplyPaging(skip, take);
        }
    }
}
