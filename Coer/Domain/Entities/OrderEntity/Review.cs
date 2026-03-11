using Domain.Entities.UsersEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderEntity
{
    public class Review : BaseEntity<int>
    {
        #region Relation Order
        public int OrderId { get; set; } //FK

        public Order Order { get; set; }
        #endregion


        public int Rating { get; set; }//1--5

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
