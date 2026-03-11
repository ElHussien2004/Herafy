using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UsersEntity
{
    public class TechnicianDocument
    {
        public string TechnicianId { get; set; } // [Fk + PK]
        
        public Technician Technician { get; set; }

        public string FaceImageUrl { get; set; }

        public string BackImageUrl { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
