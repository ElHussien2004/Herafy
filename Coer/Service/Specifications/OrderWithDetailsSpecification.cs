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
            AddInclude(x => x.Client.User);
            AddInclude(x => x.Technician);
            AddInclude(x => x.Technician.User);
        }


        public OrderWithDetailsSpecification(OrderQuery query)
            : base(x =>
             (string.IsNullOrEmpty(query.ClintId) || x.ClientId == query.ClintId) &&
             (string.IsNullOrEmpty(query.TechnicianId) || x.TechnicianId == query.TechnicianId) &&
             // حماية ضد الـ Null هنا
             (string.IsNullOrEmpty(query.NameClient) || (x.Client != null && x.Client.User != null && x.Client.User.FullName.Contains(query.NameClient))) &&
             (string.IsNullOrEmpty(query.NameTechnician) || (x.Technician != null && x.Technician.User != null && x.Technician.User.FullName.Contains(query.NameTechnician))) &&
             (!query.OrderId.HasValue || x.Id == query.OrderId) &&
             (!query.State.HasValue || x.Status == query.State)
         )
        {
            AddInclude(x => x.ServiceCategory);
            AddInclude(x => x.Client);
            AddInclude(x => x.Client.User);
            AddInclude(x => x.Technician);
            AddInclude(x => x.Technician.User); 
        }

        public OrderWithDetailsSpecification(int id) : base(O=>O.Id==id)
        {
            AddInclude(x => x.ServiceCategory);
            AddInclude(x => x.Client);
            AddInclude(x => x.Client.User);
            AddInclude(x => x.Technician);
            AddInclude(x => x.Technician.User);
        }
    }
}
