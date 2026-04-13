using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CoffeeShop.Models.Entities.System;

namespace CoffeeShop.DAL.Configurations
{
    public class SystemAuditLogConfiguration : IEntityTypeConfiguration<SystemAuditLog>
    {
        public void Configure(EntityTypeBuilder<SystemAuditLog> builder)
        {
            builder.ToTable("SystemAuditLogs");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Action).IsRequired().HasMaxLength(500);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(500);
        }
    }
}