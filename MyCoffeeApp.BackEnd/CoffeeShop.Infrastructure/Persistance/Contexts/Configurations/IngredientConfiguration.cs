/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CoffeeShop.Domain.Entities.Catalog;

namespace MyCoffeeApp.Infrastructure.Persistence.Contexts.Configurations
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("Ingredients");
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
}*/