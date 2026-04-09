using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CoffeeShop.Models.Entities.Sales;

namespace CoffeeShop.DAL.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.CreateDate).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
            builder.HasOne(u => u.Users)
                   .WithOne(u => u.UserProfile)
                   .HasForeignKey<UserProfile>(up => up.UserId)
                   .OnDelete(DeleteBehaviour.Cascade);
        }
    }
}