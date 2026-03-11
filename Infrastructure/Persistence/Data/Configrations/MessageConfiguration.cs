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
                   .HasMaxLength(2000);

          
            builder.Property(x => x.ImageUrl)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(x => x.SentAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.IsRead)
                   .HasDefaultValue(false);

            builder.Property(x => x.Type)
                   .HasDefaultValue(MessageType.Text)
                   .IsRequired()
                   .HasMaxLength(20);
            // Chat relation
            builder.HasOne(x => x.Chat)
                   .WithMany(c => c.Messages)
                   .HasForeignKey(x => x.ChatId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Sender relation
            builder.HasOne(x => x.Sender)
                   .WithMany()
                   .HasForeignKey(x => x.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.ChatId);
        }
    }
}
