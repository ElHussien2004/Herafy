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
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Rating)
                   .IsRequired();

            builder.Property(x => x.Comment)
                   .HasMaxLength(1000)
                   .IsRequired(false);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()")
                   .ValueGeneratedOnAdd();

            // Relation with Order
            builder.HasOne(x => x.Order)
                   .WithOne(o => o.Review)
                   .HasForeignKey<Review>(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.OrderId)
                   .IsUnique(); // Ensures one review per order
        }
    }
}
