using CustomerServiceCampaign.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.DataAccess.Configurations
{
    public class PersonUseCaseConfiguration : IEntityTypeConfiguration<PersonUseCase>
    {
        public void Configure(EntityTypeBuilder<PersonUseCase> builder)
        {
            builder.Property(e => e.PersonId).HasColumnName("person_id");

            builder.Property(e => e.UseCaseId).HasColumnName("use_case_id");

            builder.HasKey(e => new { e.PersonId, e.UseCaseId });
        }
    }
}
