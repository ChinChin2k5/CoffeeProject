using CoffeeShop.DAL.Interfaces; 
using CoffeeShop.DAL.Data;
using CoffeeShop.Models.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        public UserRepository (AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            // Dùng LINQ chọc xuống DB lấy User lên cực kỳ nhàn hạ
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> GetAdminAccount()
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Role == "Admin");
        }
    }
}