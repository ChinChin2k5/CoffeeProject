using Microsoft.EntityFrameworkCore;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using CoffeeShop.DAL.Repositories;
using CoffeeShop.Models.Entities.Auth;
using CoffeeShop.BLL.Interfaces;

namespace CoffeeShop.BLL.Services
{
    public class BruteForceService : IBruteForceService
    {
        private readonly BruteForceDAL _bruteForceDAL;
        public BruteForceService(BruteForceDAL bruteForceDAL)
        {
            _bruteForceDAL = bruteForceDAL;
        }
        //Hàm check xem có tài khoản nào đang bị khoá không ?
        public async Task<bool> IsAccountLocked(User user)
        {
            //Nếu có án tích (LockoutEnd có giá trị)
            if (user.LockoutEnd.HasValue)
            {
                //Nếu giờ hiện tại nhỏ hơn giờ mãn hạn tù -> Vẫn chạy lỗi
                if (DateTime.UtcNow < user.LockoutEnd.Value)
                {
                    return true;
                }
                else
                {
                    //Nếu đã ra tù (UtcNow lớn hơn LockoutEnd)
                    user.FalledLoginAttempts = 0;
                    user.LockoutEnd = null;
                    await _bruteForceDAL.UpdateUserAttemptsAsync(user);
                    return false;
                }
            }
            //Chưa bị tu đì bao giờ
            return false;
        }
        //Hàm ghi nhận 1 lần sai là 1 lần cộng dồn xuống database
        public async Task CountBruteForce(User user)
        {
            user.FalledLoginAttempts += 1;
            if (user.FalledLoginAttempts >= 5)
            {
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
            }
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
