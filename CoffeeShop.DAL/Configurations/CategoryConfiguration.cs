using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CoffeeShop.Models.Entities.Catalog;

namespace CoffeeShop.DAL.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            builder.Property(e => e.DisplayOrder).IsRequired();
            builder.Property(e => e.IsActive).IsRequired();
            builder.HasMany(u => u.Products)
                   .WithOne(u => u.Category)
                   .HasForeignKey(up => up.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}