using Microsoft.EntityFrameworkCore;
using CoffeeShop.Domain.Entities.Catalog;
using CoffeeShop.Domain.Entities.Sales;
using CoffeeShop.Domain.Entities.Inventory;
using CoffeeShop.Domain.Entities.Auth;
using CoffeeShop.Domain.Entities.System;

namespace CoffeeShop.Infrastructure.Persistence.Contexts
{
    /*public class CoffeeShopDbContext : DbContext
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
            // Nó sẽ tự tìm tất cả các class kế thừa IEntityTypeConfiguration trong project này và nạp vào.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }*/
}