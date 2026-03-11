using Domain.Entities.ServiceEntity;
using Domain.Entities.UsersEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderEntity
{
    public class Order : BaseEntity<int>
    {
        #region Relation User
        public string ClientId { get; set; } //FK From Client
        public Client Client { get; set; }
        public string TechnicianId { get; set; }//Fk From Technicaian
        public Technician Technician { get; set; }

        #endregion

        #region Relation Servic
        public int ServiceId { get; set; } //FK From Service
        public ServiceCategory ServiceCategory { get; set; }
        #endregion

        #region Relation Review
        public Review Review { get; set; }
        #endregion

        public string City { get; set; }

        public string Government { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime ScheduledDate { get; set; }

        public TimeSpan ScheduledTime { get; set; }

        public State Status { get; set; } //(pending – accepted – completed… reject)

        public decimal FinalPrice { get; set; }

        public DateTime CreatedAt { get; set; }

     
    }
}
