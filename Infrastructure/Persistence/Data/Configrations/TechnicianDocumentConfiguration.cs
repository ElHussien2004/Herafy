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
    public class TechnicianDocumentConfiguration : IEntityTypeConfiguration<TechnicianDocument>
    {
        public void Configure(EntityTypeBuilder<TechnicianDocument> builder)
        {
            builder.HasKey(x => x.TechnicianId);

            builder.Property(x => x.FaceImageUrl)
                   .IsRequired();

            builder.Property(x => x.BackImageUrl)
                   .IsRequired();
                  

            builder.Property(x => x.UploadedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Technician)
                   .WithOne(x => x.Document)
                   .HasForeignKey<TechnicianDocument>(x => x.TechnicianId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
