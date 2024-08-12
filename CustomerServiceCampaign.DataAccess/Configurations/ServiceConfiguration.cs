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
    public class ServiceConfiguration : EntityConfiguration<Service>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Service> builder)
        {
            builder.Property(e => e.ServiceName).IsRequired().HasColumnName("service_name");
            builder.HasIndex(e => e.ServiceName).IsUnique();

            builder.Property(e => e.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired()
                .HasColumnName("price");
        }
    }
}
