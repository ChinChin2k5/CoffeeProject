using CoffeeShop.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấp thẻ xanh (CORS Policy) cho cổng 5173 của Vite React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // ĐÚNG CỔNG CỦA FRONTEND NHÉ!
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddInfrastructure(builder.Configuration);

// THÊM DÒNG NÀY: Khai báo cho C# biết là "Tao có xài Controller nhé!"
builder.Services.AddControllers(); 

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Kích hoạt bảo vệ mở cổng
app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// THÊM DÒNG NÀY VÀO ĐÂY: Mở đường cho API chui vào AuthController
app.MapControllers(); 

// (Đoạn weatherforecast rác Đại ca đã dọn đi cho sạch nhà luôn rồi nhé)

app.Run();