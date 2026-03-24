
using Domain.Entities.Communications;
using Domain.Entities.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UsersEntity
{
    public class Client 
    {
        #region Relation ApplicationUser
        public string UserId { get; set; }//FK+pk
        public ApplicationUser User { get; set; }

        #endregion

        public ICollection<Order> Orders { get; set; }
        public ICollection<Chat> Chats { get; set; }

        public string City { get; set; }

        public string Government { get; set; }
        public bool IsActive { get; set; }

    }
}
