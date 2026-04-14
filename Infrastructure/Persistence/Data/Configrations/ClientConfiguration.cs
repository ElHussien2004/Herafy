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
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.City)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Government)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(false);

            builder.HasIndex(x => x.Id)
                   .IsUnique(); // One To One
        }
    }
}
