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
    public class ColorConfiguration : EntityConfiguration<Color>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Color> builder)
        {
            builder.Property(e => e.ColorName).IsRequired().HasColumnName("color_name");
            builder.HasIndex(e => e.ColorName).IsUnique();
        }
    }
}
