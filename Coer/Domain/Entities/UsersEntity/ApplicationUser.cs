using Domain.Entities.Communications;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UsersEntity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public string? ProfileImageURL { get; set; }

        public DateTime CreatedAt { get; set; }

        public Client Client { get; set; }

        public Technician Technician { get; set; }
    }
}
