using CustomerServiceCampaign.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CustomerServiceCampaign.DataAccess.Configurations
{
    public class CustomerConfiguration : EntityConfiguration<Customer>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(e => e.PersonId).HasColumnName("person_id");
            builder.HasOne(a => a.Person)
                .WithOne(p => p.Customer)
                .HasForeignKey<Customer>(a => a.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.CustomerDiscountId)
                .HasColumnName("customer_discount_id");
            builder.HasOne(a => a.CustomerDiscount)
                .WithOne(cd => cd.Customer)
                .HasForeignKey<Customer>(a => a.CustomerDiscountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
