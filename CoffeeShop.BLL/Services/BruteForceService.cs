using Microsoft.EntityFrameworkCore;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using CoffeeShop.DAL;
using CoffeeShop.Models.Entities.Auth;
using System.Threading.Tasks;
namespace CoffeeShop.BLL
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
/*public async Task<string> Login(LoginRequests request) 
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null) 
            {
                return "Email không tồn tại !";
            }
            if (request.Password != user.PasswordHash)
            {
                user.FalledLoginAttempts += 1;
                await _context.SaveChangesAsync();
                if (user.FalledLoginAttempts >= 5) 
                {
                    return "Sai 5 lần, tài khoản đã bị khoá !";
                }
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
        }*/