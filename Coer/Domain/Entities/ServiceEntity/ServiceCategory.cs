using Domain.Entities.OrderEntity;
using Domain.Entities.UsersEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ServiceEntity
{
    public class ServiceCategory:BaseEntity<int>
    {
        public string Name { get; set; }
        public ICollection<Technician> Technicians { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
