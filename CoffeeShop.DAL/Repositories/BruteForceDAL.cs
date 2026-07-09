using Microsoft.EntityFrameworkCore;
using CoffeeShop.Models.Entities.Sales;
using CoffeeShop.Models.Entities.Auth;
using CoffeeShop.DAL.Data;
namespace CoffeeShop.DAL.Repositories
{
    public class BruteForceDAL
    {
        private readonly AppDbContext _dbContext;
        public BruteForceDAL(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> UpdateUserAttemptsAsync(User user)
        {
            //Bắt buộc phải có dòng Update này
            _dbContext.Users.Update(user);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
