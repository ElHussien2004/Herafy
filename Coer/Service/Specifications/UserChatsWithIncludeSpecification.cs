using Domain.Entities.Communications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class UserChatsWithIncludeSpecification : BaseSpecifications<Chat>
    { 
        //chat  client  tech 
        // 
        public UserChatsWithIncludeSpecification(string userId)
            : base(c => c.ClientId == userId || c.TechnicianId == userId)
        {
            AddInclude(c => c.Technician);
            AddInclude(c => c.Technician.User);
            AddInclude(c => c.Client);
            AddInclude(c => c.Client.User);
            AddInclude(c => c.Messages);
            AddOrderByDescending(c => c.LastMessageAt);
        }
    }
}
