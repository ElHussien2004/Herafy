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
    public class TechnicianDocumentConfiguration : IEntityTypeConfiguration<UserDocument>
    {
        public void Configure(EntityTypeBuilder<UserDocument> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.Property(x => x.FaceImageUrl)
                   .IsRequired();

            builder.Property(x => x.BackImageUrl)
                   .IsRequired();  

            builder.Property(x => x.UploadedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.User)
                   .WithOne(U=>U.Document)
                   .HasForeignKey<UserDocument>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
