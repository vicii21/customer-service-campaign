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
    public class AddressConfiguration : EntityConfiguration<Address>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Address> builder)
        {
            builder.Property(e => e.Street).IsRequired().HasColumnName("street");

            builder.Property(e => e.Zip).HasMaxLength(5).HasColumnName("zip");

            builder.Property(e => e.CityId).HasColumnName("city_id");

            builder.HasOne(e => e.City)
                .WithMany(c => c.Addresses)
                .HasForeignKey(c => c.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.StateId).HasColumnName("state_id");

            builder.HasOne(e => e.State)
                .WithMany(s => s.Addresses)
                .HasForeignKey(e => e.StateId)
                .IsRequired();
        }
    }
}
