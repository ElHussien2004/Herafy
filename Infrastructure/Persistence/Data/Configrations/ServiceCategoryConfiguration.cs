using Domain.Entities.ServiceEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configrations
{
    public class ServiceCategoryConfiguration : IEntityTypeConfiguration<ServiceCategory>
    {
        public void Configure(EntityTypeBuilder<ServiceCategory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired();

            

            // Relation with Technicians
            builder.HasMany(x => x.Technicians)
                   .WithOne(t => t.ServiceCategory)
                   .HasForeignKey(t => t.ServiceCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relation with Orders
            builder.HasMany(x => x.Orders)
                   .WithOne(o => o.ServiceCategory)
                   .HasForeignKey(o => o.ServiceId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
