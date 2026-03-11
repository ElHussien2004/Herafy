using Domain.Entities.UsersEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configrations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.ProfileImageURL)
                   .IsRequired(false);

            builder.Property(x => x.CreatedAt)
                       .HasDefaultValueSql("GETDATE()");
            
            // One To One -> Client
            builder.HasOne(x => x.Client)
                   .WithOne(x => x.User)
                   .HasForeignKey<Client>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // One To One -> Technician
            builder.HasOne(x => x.Technician)
                   .WithOne(x => x.User)
                   .HasForeignKey<Technician>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
