using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CustomerServiceCampaign.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace CustomerServiceCampaign.DataAccess.Configurations
{
    public class PersonConfiguration : EntityConfiguration<Person>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Person> builder)
        {
            builder.Property(e => e.Name).IsRequired().HasColumnName("name");

            builder.Property(e => e.DOB).IsRequired().HasColumnName("date_of_birth");

            builder.Property(e => e.Age).IsRequired().HasColumnName("age");

            builder.Property(e => e.SSN)
                .HasColumnType("char(11)")
                .IsRequired()
                .HasColumnName("social_security_number");
            builder.HasIndex(e => e.SSN).IsUnique();

            builder.Property(e => e.HomeAddressId).HasColumnName("house_address_id");
            builder.HasOne(e => e.HomeAddress)
                .WithMany(ha => ha.HomePersons)
                .HasForeignKey(e => e.HomeAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.OfficeAddressId).HasColumnName("office_address_id");
            builder.HasOne(e => e.OfficeAddress)
                .WithMany(oa => oa.OfficePersons)
                .HasForeignKey(e => e.OfficeAddressId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.SpouseId).HasColumnName("spouse_id");
            builder.HasOne(p => p.Spouse)
                .WithOne()
                .HasForeignKey<Person>(p => p.SpouseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(e => e.SpouseId).IsUnique();
        }
    }
}
