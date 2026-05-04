using Domain.Entities.Communications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class ChatWithPartiesSpecification:BaseSpecifications<Chat>
    {
        public ChatWithPartiesSpecification(string clientid,string TecId):base(C=>C.TechnicianId==TecId && C.ClientId==clientid)
        {
            AddInclude(c => c.Technician);
            AddInclude(c => c.Client);
            AddInclude(c => c.Technician.User);
            AddInclude(c => c.Client.User);
        }
        
    }
}
