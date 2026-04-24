using Domain.Entities.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class OrderQuery
    {
        public string? ClintId { get; set; }
        public string?TechnicianId { get; set; }

        public string? NameClient { get; set; }
        public string? NameTechnician { get; set; }
        public State? State { get; set; }
        public int? OrderId { get; set; }

    }
}
