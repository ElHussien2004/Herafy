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
    public class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
    {
        public void Configure(EntityTypeBuilder<Complaint> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                   .HasMaxLength(400);

            builder.Property(c => c.Description)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.Property(c => c.Response)
                   .HasMaxLength(2000);

            builder.Property(c => c.CreatedAt)
                   .HasDefaultValueSql("GETDATE()")
                   .ValueGeneratedOnAdd();

            builder.Property(c => c.Status)
                   .HasConversion<string>()
                   .IsRequired();

            builder.HasOne(c => c.Order)
                   .WithMany(o => o.Complaints)
                   .HasForeignKey(c => c.OrderId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(c => c.User)
                   .WithMany(u => u.Complaints)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
