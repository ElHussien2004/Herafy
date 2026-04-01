
using Domain.Entities.Communications;
using Domain.Entities.OrderEntity;
using Domain.Entities.ServiceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UsersEntity
{
    public class Technician :BaseEntity<string>
    {
        #region Relation User
        public string UserId { get; set; }//Fk +pk
        public ApplicationUser User { get; set; }
        #endregion

        #region Relation Servic
         public int ServiceCategoryId { get; set; } // FK
         public ServiceCategory ServiceCategory { get; set; }
        #endregion

        #region Relation Order
        public ICollection<Order> Orders { get; set; }
        #endregion

        #region Relation Chat
        public ICollection<Chat> Chats { get; set; }
        #endregion
        public int ExperienceYears { get; set; }

        public bool AvailabilityStatus { get; set; }//هو متاح للطلب ولا لا 

        public double RatingAvg { get; set; }

        public string Bio { get; set; }

        public decimal InspectedPrice { get; set; }

        public int CompletedJobs { get; set; }

        public string City { get; set; }

        public string Government { get; set; }

        public bool IsActive { get; set; }
        public double Latitude { get; set; }

        public double Longitude { get; set; }
        public TechnicianDocument Document { get; set; }
    }
}
