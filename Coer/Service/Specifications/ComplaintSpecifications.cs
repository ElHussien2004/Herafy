using Domain.Entities.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class ComplaintSpecifications : BaseSpecifications<Complaint>
    {
        public ComplaintSpecifications(int id) : base(c => c.Id == id)
        {
            AddInclude(c => c.User);
            AddInclude(c => c.Order);
        }
        public ComplaintSpecifications() : base(c => true)
        {
            AddInclude(c => c.User);
            AddInclude(c => c.Order);
        }
        public ComplaintSpecifications(string userId)
            : base(c => c.UserId == userId)
        {
            AddInclude(c => c.User);
            AddInclude(c => c.Order);
        }

        public ComplaintSpecifications(ComplaintStatus status)
            : base(c => c.Status == status)
        {
            AddInclude(c => c.User);
            AddInclude(c => c.Order);
        }
        public ComplaintSpecifications(int orderId, bool byOrder)
            : base(c => c.OrderId == orderId)
        {
            AddInclude(c => c.User);
            AddInclude(c => c.Order);
        }
    }
}
