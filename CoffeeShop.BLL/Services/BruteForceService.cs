using Microsoft.EntityFrameworkCore;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using CoffeeShop.DAL.Repositories;
using CoffeeShop.Models.Entities.Auth;
namespace CoffeeShop.BLL.Services
{
    public class BruteForceService
    {
        private readonly BruteForceDAL _bruteForceDAL;
        public BruteForceService(BruteForceDAL bruteForceDAL)
        {
            _bruteForceDAL = bruteForceDAL;
        }
        //Hàm check xem có tài khoản nào đang bị khoá không ?
        public bool IsAccountLocked(User user)
        {
            return user.FalledLoginAttempts >= 5;
        }
        //Hàm ghi nhận 1 lần sai là 1 lần cộng dồn xuống database
        public async Task CountBruteForce(User user)
        {
            user.FalledLoginAttempts += 1;
            await _bruteForceDAL.UpdateUserAttemptsAsync(user);
        }
        public async Task ResetFalledAttemptAsync(User user)
        {
            if (user.FalledLoginAttempts > 0)
            {
                user.FalledLoginAttempts = 0;
                await _bruteForceDAL.UpdateUserAttemptsAsync(user);
            }
        }
    }
}
