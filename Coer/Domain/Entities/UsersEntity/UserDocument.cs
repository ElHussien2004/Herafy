using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UsersEntity
{
    public class UserDocument
    {
        public string UserId { get; set; } // [Fk + PK]
        
        public ApplicationUser User { get; set; }

        public string FaceImageUrl { get; set; }

        public string BackImageUrl { get; set; }

        public DateTime UploadedAt { get; set; }

    }
}
