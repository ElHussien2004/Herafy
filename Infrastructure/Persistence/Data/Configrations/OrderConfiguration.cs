using Domain.Entities.OrderEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configrations
{
    public class OrderConfiguration: IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.City)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Government)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Latitude)
                   .HasColumnType("decimal(9,6)")
                   .IsRequired();

            builder.Property(x => x.Longitude)
                   .HasColumnType("decimal(9,6)")
                   .IsRequired();
            

            builder.Property(x => x.ScheduledDate)
                   .IsRequired();

            builder.Property(x => x.ScheduledTime)
                   .IsRequired();

            builder.Property(x => x.FinalPrice)
                   .HasColumnType("decimal(10,2)");

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()")
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.Status)
                   .HasConversion<int>() // Enum State to int
                   .IsRequired();

            // Relation with Client
            builder.HasOne(x => x.Client)
                   .WithMany(c => c.Orders)
                   .HasForeignKey(x => x.ClientId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relation with Technician
            builder.HasOne(x => x.Technician)
                   .WithMany(t => t.Orders)
                   .HasForeignKey(x => x.TechnicianId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relation with ServiceCategory
            builder.HasOne(x => x.ServiceCategory)
                   .WithMany(s => s.Orders)
                   .HasForeignKey(x => x.ServiceId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relation with Review (One-to-One)
            builder.HasOne(x => x.Review)
                   .WithOne(r => r.Order)
                   .HasForeignKey<Review>(r => r.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
