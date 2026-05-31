using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CoffeeShop.Models.Entities.Sales;

namespace CoffeeShop.DAL.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");
            // 1. Trả lại vị trí Khóa Chính cho Id
            builder.HasKey(e => e.Id); 
            
            // 2. OrderId hạ cấp xuống làm thuộc tính bắt buộc (Khóa ngoại)
            builder.Property(e => e.OrderId).IsRequired();
            builder.Property(e => e.ProductId).IsRequired();
            builder.Property(e => e.Quantity).IsRequired();
            builder.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
        }
    }
}