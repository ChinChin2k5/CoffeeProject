using Microsoft.EntityFrameworkCore;
using CoffeeShop.Models.Entities.Auth;
using CoffeeShop.Models.Entities.Catalog;
using CoffeeShop.Models.Entities.Inventory;
using CoffeeShop.Models.Entities.Sales;
using CoffeeShop.Models.Entities.System;



namespace CoffeeShop.DAL.Data 
{
    public class AppDbContext : DbContext 
    {
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Bước "lấy khung" của Bố 
            base.OnModelCreating(modelBuilder);
            // Nó sẽ tự tìm tất cả các class kế thừa IEntityTypeConfiguration trong project này và nạp vào.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}