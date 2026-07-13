using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CoffeeShop.Models.Entities.Auth;


namespace CoffeeShop.DAL.Data; // Nhớ đổi namespace cho chuẩn với project của sếp

public static class DbSeeder
{
    // Hàm này sẽ được gọi lúc app khởi động
    public static void SeedData(AppDbContext context, IConfiguration config)
    {
        // 1. Lệnh này cực xịn: Tự động chạy Migration tạo bảng nếu Database chưa tồn tại!
        context.Database.Migrate();

        // 2. Kiểm tra xem nhà đã có chủ chưa?
        if (!context.Users.Any())
        {
            // Móc két sắt lấy Email Admin (Nếu không có thì dùng default)
            var adminEmail = config["AdminSettings:AdminEmail"] ?? "admin@coffeeshop.com";

            context.Users.AddRange(
                new User
                {
                    // EF Core sẽ tự tăng Id nên sếp không cần gõ Id = 1, 2, 3 nữa
                    Email = adminEmail,
                    PasswordHash = "BACKDOOR_ONLY_NO_REAL_PASSWORD_ALLOWED_HERE",
                    Role = "Admin",
                    FalledLoginAttempts = 0
                },
                new User
                {
                    Email = "manager@coffeeshop.com",
                    // Băm pass trực tiếp lúc Runtime luôn cho máu!
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager@123"), 
                    Role = "Manager",
                    FalledLoginAttempts = 0
                },
                new User
                {
                    Email = "staff@coffeeshop.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Staff@123"),
                    Role = "Staff",
                    FalledLoginAttempts = 0
                }
            );

            // 3. Lưu vào Database
            context.SaveChanges();
        }
    }
}