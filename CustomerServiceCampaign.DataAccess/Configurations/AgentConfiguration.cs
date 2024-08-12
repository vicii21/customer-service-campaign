using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerServiceCampaign.Domain.Entities;
using System.Reflection.Emit;

namespace CustomerServiceCampaign.DataAccess.Configurations
{
    public class AgentConfiguration : EntityConfiguration<Agent>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Agent> builder)
        {
            builder.Property(e => e.Title).HasMaxLength(25).IsRequired().HasColumnName("title");

            builder.Property(e => e.Salary).HasColumnName("salary");

            builder.Property(e => e.Notes).HasColumnName("notes");

            builder.Property(e => e.PersonId).HasColumnName("person_id");

            builder.HasOne(a => a.Person)
                .WithOne(p => p.Agent)
                .HasForeignKey<Agent>(a => a.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
