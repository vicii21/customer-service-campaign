using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerServiceCampaign.Domain.Entities;

namespace CustomerServiceCampaign.DataAccess.Configurations
{
    public class LogEntryConfiguration : EntityConfiguration<LogEntry>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<LogEntry> builder)
        {
            builder.Property(e => e.ActorId).IsRequired().HasColumnName("actor_id");

            builder.Property(e => e.Actor).IsRequired().HasColumnName("actor");

            builder.Property(e => e.UseCaseName).HasMaxLength(5).HasColumnName("use_case_name");

            builder.Property(e => e.UseCaseData).HasMaxLength(5).HasColumnName("use_case_data");
        }
    }
}
