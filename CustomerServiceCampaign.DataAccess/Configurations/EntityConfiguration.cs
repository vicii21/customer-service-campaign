using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerServiceCampaign.Domain.Entities;

namespace CustomerServiceCampaign.DataAccess.Configurations
{
    public abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.ID).HasColumnName("id");

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .HasColumnName("created_at");

            builder.Property(e => e.IsActive)
                .HasDefaultValueSql("1")
                .HasColumnName("is_active");

            ConfigureEntity(builder);
        }

        protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
    }
}
