using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCoffeeApp.Domain.Entities.Auth;

namespace MyCoffeeApp.Infrastructure.Persistence.Contexts.Configurations
{
    public class SystemAuditLogConfiguration : IEntityTypeConfiguration<SystemAuditLog>
    {
        public void Configure(EntityTypeBuilder<SystemAuditlog> builder)
        {
            builder.ToTable("SystemAuditLogs");
            builder.HasKey(e => e.UserId);
            builder.Property(e => e.FullName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Avatar).IsRequired().HasMaxLength(500);
            builder.HasOne(u => u.Users)
                   .WithOne(u => u.UserProfile)
                   .HasForeignKey<UserProfile>(up => up.UserId)
                   .OnDelete(DeleteBehaviour.Cascade);
        }
    }
}