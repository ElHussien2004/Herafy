using Domain.Entities.Communications;
using Domain.Entities.OrderEntity;
using Domain.Entities.ServiceEntity;
using Domain.Entities.UsersEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class HerafyDbContext(DbContextOptions<HerafyDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        #region Dbsets
        public DbSet<Client> Clients { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet <Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet <ServiceCategory> ServiceCategories { get; set; }
        public DbSet<TechnicianDocument> Documents { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet <Chat> Chats { get; set; }
        public DbSet <Message > Messages { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AssemblyRefrance).Assembly);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Ignore<IdentityUserClaim<string>>();
            builder.Ignore<IdentityUserToken<string>>();
            builder.Ignore<IdentityUserLogin<string>>();
            builder.Ignore<IdentityRoleClaim<string>>();
        }
    }
}
