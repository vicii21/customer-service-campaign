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
    public class OrderConfiguration : EntityConfiguration<Order>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Order> builder)
        {
            builder.Property(e => e.CustomerDiscountId)
                .IsRequired()
                .HasColumnName("customer_discount_id");
            builder.HasOne(e => e.CustomerDiscount)
                .WithMany(cd => cd.Orders)
                .HasForeignKey(e => e.CustomerDiscountId);

            builder.Property(e => e.ServiceId)
                .IsRequired()
                .HasColumnName("service_id");
            builder.HasOne(e => e.Service)
                .WithMany(a => a.Orders)
                .HasForeignKey(e => e.ServiceId);
        }
    }
}
