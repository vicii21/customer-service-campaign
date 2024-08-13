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
    public class CityConfiguration : EntityConfiguration<City>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<City> builder)
        {
            builder.Property(e => e.CityName).IsRequired().HasColumnName("city_name");
        }
    }
}
