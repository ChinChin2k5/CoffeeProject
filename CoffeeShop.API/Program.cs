using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
//Chống bruteforce
using Microsoft.AspNetCore.RateLimiting;
using CoffeeShop.DAL.Data;         
using CoffeeShop.DAL.Repositories; 
using CoffeeShop.DAL.Interfaces;   
using CoffeeShop.BLL.Services;     
using CoffeeShop.BLL.Interfaces;   

var builder = WebApplication.CreateBuilder(args);

// --- 1. ĐĂNG KÝ CỔNG ---
builder.Services.AddControllers();
builder.Services.AddScoped<BruteForceService>();
// Lấy thông tin cấu hình JWT từ file appsetiings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("Secret Key của JWT chưa được cấu hình, không thể chạy app!");
}
// 2. Cấu hình Authentication Service với JWT Bearer
builder.Services.AddAuthentication(options =>
{
    // Đặt mặc định khi API nhận request sẽ dùng cơ chế JWT Bearer để check
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Định nghĩa các quy tắc để kiểm tra xem Token gửi lên có hợp lệ không
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,         // Kiểm tra xem Token có đúng do Server mình phát hành không
        ValidateAudience = true,       // Kiểm tra xem Token có gửi đúng đến Client được phép không
        ValidateLifetime = true,       // Kiểm tra xem Token còn hạn sử dụng không
        ValidateIssuerSigningKey = true, // Kiểm tra chữ ký bảo mật để tránh Token giả mạo

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        
        // "Bẫy" Junior hay dính: Mặc định .NET Core sẽ cộng thêm 5 phút bù trừ chênh lệch thời gian (ClockSkew).
        // Đặt về Zero để Token hết hạn chính xác từng giây theo cấu hình.
        ClockSkew = TimeSpan.Zero, 
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["accessToken"];
            return Task.CompletedTask;
        }
    };
});
// 3. Cấu hình Authorization Service (Phân quyền nâng cao bằng Policy)
builder.Services.AddAuthorization(options =>
{
    // Tạo một Policy tên là "AdminOnly", bắt buộc user phải có Role là "Admin"
    // "Role" là một Claims đặc biệt
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Admin","Manager"));
    options.AddPolicy("StaffOnly", policy => policy.RequireRole("Admin","Manager","Staff"));
});
builder.Services.AddRateLimiter(options => 
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("fixed",fixedOptions => {
        fixedOptions.PermitLimit = 5;
        fixedOptions.Window = TimeSpan.FromSeconds(10);
    });
});
builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowViteApp", policy => 
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod() //Cho phép thích làm gì thì làm
              .AllowCredentials(); //Cho phép Client gửi Cookie lên Server
    });
});

// Đăng ký Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<EmailService>();
//builder.Services.AddScoped<RecoveryService>();
builder.Services.AddScoped<OrderDAL>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductDAL>();
builder.Services.AddScoped<StaffService>();
builder.Services.AddScoped<StaffDAL>();
builder.Services.AddScoped<BruteForceDAL>();
builder.Services.AddScoped<PasswordHasher>();
// "Hễ ai đòi IUserRepository, hãy đưa cho nó class UserRepository"
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IBruteForceService, BruteForceService>();


// "Hễ ai đòi IAuthService, hãy đưa cho nó class AuthService"
builder.Services.AddScoped<IAuthService, AuthService>();
// CoffeeShop.BLL.TokenService từ trái qua là namespace tức hộ khẩu của nó !
builder.Services.AddScoped<TokenService>();





var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Mặc định nó sẽ tự tìm đến /swagger
}
//Cho phép riêng cổng 5173 được chạy cùng cổng 5079 của backend
app.UseCors("AllowViteApp");

app.UseHttpsRedirection();
// Gọi hàm chống bruteforce
app.UseRateLimiter();
// Trả lời câu hỏi bạn là ai (Xác thực)
app.UseAuthentication();
// Trả lời câu hỏi bạn làm được gì (Phân quyền)
app.UseAuthorization();


app.MapControllers(); 

//Kích hoạt Seeding Động
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var config = services.GetRequiredService<IConfiguration>();
        
        // Bấm nút khởi động máy bơm!
        DbSeeder.SeedData(context, config);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Có biến lúc bơm dữ liệu Seeding rồi sếp ơi!");
    }
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Nhớ đổi chữ AppDbContext thành tên DB Context thực tế của sếp nhé
        var context = services.GetRequiredService<AppDbContext>(); 
        
        // GỌI HÀM SEED Ở ĐÂY NÀY!
        DbInitializer.Seed(context); 
    }
    catch (Exception ex)
    {
        Console.WriteLine("Oái! Lỗi lúc Seeding sếp ơi: " + ex.Message);
    }
}
// --- 3. KHỞI CHẠY ---
app.Run();