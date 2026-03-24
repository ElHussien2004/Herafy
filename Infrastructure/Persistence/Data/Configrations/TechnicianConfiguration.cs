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
    public class TechnicianConfiguration: IEntityTypeConfiguration<Technician>
    {
        public void Configure(EntityTypeBuilder<Technician> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.Property(x => x.Bio)
                   .HasMaxLength(1000);

            builder.Property(x => x.InspectedPrice)
                   .HasColumnType("decimal(10,2)");

            builder.Property(x => x.RatingAvg)
                   .HasDefaultValue(0);

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(false);

            builder.Property(x => x.CompletedJobs)
                   .HasDefaultValue(0);

            builder.HasIndex(x => x.UserId)
                   .IsUnique();

            // Relation with ServiceCategory
            builder.HasOne(x => x.ServiceCategory)
                   .WithMany(x => x.Technicians)
                   .HasForeignKey(x => x.ServiceCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
