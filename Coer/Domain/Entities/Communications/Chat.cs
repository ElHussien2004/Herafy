using Domain.Entities.OrderEntity;
using Domain.Entities.UsersEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Communications
{
    public class Chat : BaseEntity<int>
    {
        public string ClientId { get; set; }
        public Client Client { get; set; }
        public string TechnicianId { get; set; }
        public Technician Technician { get; set; }
        public DateTime LastMessageAt { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
