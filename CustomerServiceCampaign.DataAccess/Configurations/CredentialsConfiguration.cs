using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CustomerServiceCampaign.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.DataAccess.Configurations
{
    public class CredentialsConfiguration : EntityConfiguration<Credentials>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Credentials> builder)
        {
            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("email");
            builder.HasIndex(e => e.Email).IsUnique();

            builder.Property(e => e.PersonId).HasColumnName("person_id");
            builder.HasOne(e => e.Person)
                .WithOne(p => p.Credentials)
                .HasForeignKey<Credentials>(e => e.PersonId);

            builder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(80)
                .HasColumnName("password");
        }
    }
}
