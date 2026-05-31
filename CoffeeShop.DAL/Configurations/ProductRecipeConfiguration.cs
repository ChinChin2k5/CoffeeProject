using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CoffeeShop.Models.Entities.Catalog;

namespace CoffeeShop.DAL.Configurations
{
    public class ProductRecipeConfiguration : IEntityTypeConfiguration<ProductRecipe>
    {
        public void Configure(EntityTypeBuilder<ProductRecipe> builder)
        {
            builder.ToTable("ProductRecipes");
            builder.HasKey(e => e.ProductId);
            builder.Property(e => e.ItemId).IsRequired();
            builder.Property(e => e.QuantityNeeded).IsRequired();
        }
    }
}