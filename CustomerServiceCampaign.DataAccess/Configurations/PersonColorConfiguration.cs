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
    public class PersonColorConfiguration : IEntityTypeConfiguration<PersonColor>
    {
        public void Configure(EntityTypeBuilder<PersonColor> builder)
        {
            builder.HasKey(e => new { e.PersonId, e.ColorId });
        }
    }
}
