using Domain.Entities.UsersEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    class ClientSpecifications:BaseSpecifications<Client>
    {
        public ClientSpecifications(string? id):base(C=>C.UserId==id)
        {
            AddInclude(C => C.User);
        }
        public ClientSpecifications():base(null)
        {
            AddInclude(C => C.User);
        }
    }
}
