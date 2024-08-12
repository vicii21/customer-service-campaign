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
    public class CustomerDiscountConfiguration : EntityConfiguration<CustomerDiscount>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<CustomerDiscount> builder)
        {
            builder.Property(e => e.DiscountValue).HasDefaultValue(15);

            builder.Property(e => e.IsUsed).IsRequired();

            builder.Property(e => e.DiscountExpires).IsRequired();

            builder.HasOne(e => e.Customer)
                .WithOne(c => c.CustomerDiscount)
                .HasForeignKey<CustomerDiscount>(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.CustomerId).IsUnique();

            builder.HasOne(e => e.Agent)
                .WithMany(a => a.CustomerDiscounts)
                .HasForeignKey(e => e.AgentId);
        }
    }
}
