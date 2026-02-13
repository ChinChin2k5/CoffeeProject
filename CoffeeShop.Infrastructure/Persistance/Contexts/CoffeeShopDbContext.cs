using Microsoft.EntityFrameworkCore;
using CoffeeShop.Domain.Entities.Catalog;
using CoffeeShop.Domain.Entities.Sales;
using CoffeeShop.Domain.Entities.Inventory;
using CoffeeShop.Domain.Entities.Auth;
using CoffeeShop.Domain.Entities.System;

namespace CoffeeShop.Infrastructure.Persistence.Contexts
{
    public class CoffeeShopDbContext : DbContext
    {
        public CoffeeShopDbContext(DbContextOptions<CoffeeShopDbContext> options) : base(options)
        {}
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductRecipe> ProductRecipes { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreInventory> StoreInventories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<SystemAuditLog> SystemAuditLogs { get; set; }
        //Nó chỉ là cái khung rỗng thôi, bây giờ mình sẽ lấy cái khung này chứa đa ta bây
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Bước "lấy khung" của Bố 
            base.OnModelCreating(modelBuilder);
            modeilBuilder.Entity<User>(entity => 
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(10).HasDefaultValueSql("Staff");
                entity.Property(e => e.FalledLoginAttempts).HasDefaultValue(0);
                entity.Property(e => e.LockoutEnd).IsRequired(false); //Cho phép null
                entity.HasOne(u => u.Store)
                      .WithMany(s => s.Users)
                      .HasForeignKey(u => u.StoreId)
                      .OnDelete(DeleteBehavior.Restrict);s
            })
        }
    }
}