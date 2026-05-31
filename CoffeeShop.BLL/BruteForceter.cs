using Microsoft.EntityFrameworkCore;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using CoffeeShop.DAL;
using CoffeeShop.Models.Entities.Auth;
using System;
namespace CoffeeShop.BLL.BruteForceter
{
    public class AgainstBruteForce 
    {
        // Khai báo DbContext để tương tác với CSDL
        private readonly AppDbContext _context;
        // Nạp DbContext vào class
        public AgainstBruteForce(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string> Login(LoginRequests request) 
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null) 
            {
                return "Email không tồn tại !";
            }
            if (request.Password != user.PasswordHash)
            {
                user.FalledLoginAttempts += 1;
                if (user.FalledLoginAttempts >= 5) 
                {
                    return "Sai 5 lần, tài khoản đã bị khoá !";
                }
                await _context.SaveChangesAsync();
                return "Sai mật khẩu!";
            }
            if (user.Role.ToLower() != request.Role.ToLower())
            {
                return "Bạn không có quyền đăng nhập vào cổng này!";
            }
            user.FalledLoginAttempts = 0;
            await _context.SaveChangesAsync();
            return "Đăng nhập thành công! Nhả Token ra đây!";
        }
        public async Task<string> Response(LoginResponses response)
        {
            return null; //Tam thoi chua xu ly
        }
    }
}