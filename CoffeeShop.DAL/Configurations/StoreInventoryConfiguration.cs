using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CoffeeShop.Models.Entities.Inventory;

namespace CoffeeShop.DAL.Configurations
{
    public class StoreInventoryConfiguration : IEntityTypeConfiguration<StoreInventory>
    {
        public void Configure(EntityTypeBuilder<StoreInventory> builder)
        {
            builder.ToTable("StoreInventories");
            builder.HasKey(si => new { si.StoreId, si.ItemId });
            builder.Property(e => e.ItemId).IsRequired();
            builder.Property(e => e.Quantity).IsRequired();
        }
    }
}