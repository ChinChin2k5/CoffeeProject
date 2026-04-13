using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.DAL.Data; // Chứa CafeDBContext của em

var builder = WebApplication.CreateBuilder(args);

// --- 1. GIAI ĐOẠN CHUẨN BỊ (ĐĂNG KÝ DỊCH VỤ) ---

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Bắt bug: Đổi AppDbContext thành CafeDBContext cho khớp với file nãy em tạo
builder.Services.AddDbContext<CafeDBContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

// --- 2. GIAI ĐOẠN ĐÓNG GÓI (LỆNH BULD EM VỪA PHÁT HIỆN) ---
var app = builder.Build();

// --- 3. GIAI ĐOẠN CẤU HÌNH LUỒNG ĐI (MIDDLEWARE) ---
// Thằng này cực kỳ quan trọng, không có nó thì API không biết Route đường dẫn đi đâu
app.MapControllers(); 

// --- 4. BẤM NÚT START ĐỘNG CƠ ---
app.Run();