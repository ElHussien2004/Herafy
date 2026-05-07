using Domain.Entities.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class ReviewSpecification : BaseSpecifications<Review>
    {
        public ReviewSpecification(string TechnicianId) : base(R=>R.Order.TechnicianId==TechnicianId 
        &&R.IsApproved==true&& R.is_suspicious==false)
        {
            AddInclude(r => r.Order);
            AddInclude(r => r.Order.Client.User);
            AddInclude(r => r.Order.Technician.User);
            AddInclude(r => r.Order.Client);
            AddInclude(r => r.Order.Technician);
        }
        public ReviewSpecification() : base(R=>R.is_suspicious==true)
        {
            AddInclude(r => r.Order);
            AddInclude(r => r.Order.Client.User);
            AddInclude(r => r.Order.Technician.User);
            AddInclude(r => r.Order.Client);
            AddInclude(r => r.Order.Technician);
        }
        public ReviewSpecification(int id) : base(r=>r.Id==id)
        {
            AddInclude(r => r.Order);
            AddInclude(r => r.Order.Client.User);
            AddInclude(r => r.Order.Technician.User);
            AddInclude(r => r.Order.Client);
            AddInclude(r => r.Order.Technician);
        }
        public ReviewSpecification(bool Approved) : base(R => R.IsApproved == Approved)
        {
            AddInclude(r => r.Order);
            AddInclude(r => r.Order.Client.User);
            AddInclude(r => r.Order.Technician.User);
            AddInclude(r => r.Order.Client);
            AddInclude(r => r.Order.Technician);
        }

    }
}
