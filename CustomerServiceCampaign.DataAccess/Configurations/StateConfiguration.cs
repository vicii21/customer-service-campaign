using CustomerServiceCampaign.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.DataAccess.Configurations
{
    public class StateConfiguration : EntityConfiguration<State>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<State> builder)
        {
            builder.Property(e => e.StateName).IsRequired().HasColumnName("state_name");
        }
    }
}
