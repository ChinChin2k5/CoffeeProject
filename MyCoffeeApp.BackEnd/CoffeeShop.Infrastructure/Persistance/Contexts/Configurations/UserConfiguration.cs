using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CoffeeShop.Domain.Entities.Auth;

namespace MyCoffeeApp.Infrastructure.Persistence.Contexts.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            /*builder.ToTable("Users");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Username).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
            builder.Property(e => e.Role).IsRequired().HasMaxLength(10).HasDefaultValueSql("Staff");
            builder.Property(e => e.FalledLoginAttempts).HasDefaultValue(0);
            builder.Property(e => e.LockoutEnd).IsRequired(false); //Cho phép null
            builder.HasOne(u => u.Store)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.StoreId)
                .OnDelete(DeleteBehavior.Restrict);*/
        }
    }
}