using Domain.Entities.OrderEntity;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Service.Specifications
{
    public class OrderWithDetailsSpecification: BaseSpecifications<Order>
    {
        public OrderWithDetailsSpecification():base(null)
        {
            AddInclude(x => x.ServiceCategory);
            AddInclude(x => x.Client);
            AddInclude(x => x.Technician);
        }

        
        public OrderWithDetailsSpecification(OrderQuery query)
           : base(x =>
            (string.IsNullOrEmpty(query.ClintId) || x.ClientId == query.ClintId) &&

            (string.IsNullOrEmpty(query.TechnicianId) || x.TechnicianId == query.TechnicianId) &&

            (string.IsNullOrEmpty(query.NameClient) || x.Client.User.FullName.Contains(query.NameClient)) &&

            (string.IsNullOrEmpty(query.NameTechnician) || x.Technician.User.FullName.Contains(query.NameTechnician)) &&

            (!query.OrderId.HasValue || x.Id == query.OrderId) &&

           (!query.State.HasValue || x.Status == query.State)
        )
        {
            AddInclude(x => x.ServiceCategory);
            AddInclude(x => x.Client);
            AddInclude(x => x.Technician);
            
        }

        
    }
}
