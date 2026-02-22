using CoffeeShop.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeShop.Infrastructure
{
    public static class DependencyInjection
    {
        //Gọi ra hàm IServiceCollection của Microsoft
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Đăng ký DbContext dùng PostgreSQL
            /*services.AddDbContext<CoffeeShopDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));*/
            return services;
        }
    }
}