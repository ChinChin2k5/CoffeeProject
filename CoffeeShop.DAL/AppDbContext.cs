using Microsoft.EntityFrameworkCore;


namespace CoffeeShop.DAL.Data 
{
    public class CafeDBContext : DbContext 
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Bước "lấy khung" của Bố 
            base.OnModelCreating(modelBuilder);
            // Nó sẽ tự tìm tất cả các class kế thừa IEntityTypeConfiguration trong project này và nạp vào.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}