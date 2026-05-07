using Domain.Entities.UsersEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderEntity
{
    public class Complaint : BaseEntity<int>
    {
        // FK Order
        public int? OrderId { get; set; }
        public Order? Order { get; set; }

        // FK User
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string? Title { get; set; }
        public string Description { get; set; }
        public string? Response { get; set; }=string.Empty;
        public DateTime CreatedAt { get; set; }
        public ComplaintStatus Status { get; set; }
    }
}
