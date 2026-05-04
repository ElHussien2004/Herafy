using Domain.Entities.Communications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configrations
{
    public class MessageConfiguration: IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Content)
                   .HasMaxLength(4000);


            builder.Property(x => x.SentAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.IsRead)
                   .HasDefaultValue(false);

            builder.Property(x => x.Type)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(x => x.Chat)
                 .WithMany(c => c.Messages)
                 .HasForeignKey(x => x.ChatId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Sender)
                   .WithMany()
                   .HasForeignKey(x => x.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Performance: أهم Index لجلب تاريخ الشات (History)
            builder.HasIndex(x => new { x.ChatId, x.SentAt });
        }
    }
}
