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
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.LastMessageAt)
                .HasDefaultValueSql("GETDATE()");

            // علاقة العميل (Client)
            builder.HasOne(x => x.Client)
                   .WithMany(c => c.Chats) 
                   .HasForeignKey(x => x.ClientId)
                   .OnDelete(DeleteBehavior.Restrict);

            // علاقة الفني (Technician)
            builder.HasOne(x => x.Technician)
                   .WithMany(t => t.Chats) 
                   .HasForeignKey(x => x.TechnicianId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Messages Relation
            builder.HasMany(x => x.Messages)
                   .WithOne(m => m.Chat)
                   .HasForeignKey(m => m.ChatId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Index لضمان عدم تكرار شات بين نفس الفني والعميل
            builder.HasIndex(x => new { x.ClientId, x.TechnicianId })
                   .IsUnique();
        }
    }
}
