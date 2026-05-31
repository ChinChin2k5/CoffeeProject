using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CoffeeShop.Models.Entities.Sales;
using CoffeeShop.Models.Entities.Auth;

namespace CoffeeShop.DAL.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.CreateDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(e => e.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
            // Mapping quan hệ 1-N chuẩn sách giáo khoa
            builder.HasMany(o => o.OrderDetails)
                   .WithOne(od => od.Order) // Điền rõ navigation property vào đây
                   .HasForeignKey(od => od.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }

    }
}