using CoffeeShop.DAL.Interfaces; 
using CoffeeShop.DAL.Data;
using CoffeeShop.Models.Entities.Auth;

namespace CoffeeShop.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        public UserRepository (AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public User GetUserByEmail(string email)
        {
            // Dùng LINQ chọc xuống DB lấy User lên cực kỳ nhàn hạ
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}