using CoffeeShop.DAL.Repositories;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using CoffeeShop.BLL;

// Nên có cái namespace để bọc class lại cho chuẩn kiến trúc nhé
namespace CoffeeShop.BLL.Services
{
    public class RecoveryService 
    {
        private readonly EmailService _email;
        private readonly AccountRepository _account;

        public RecoveryService(EmailService email, AccountRepository account)
        {
            _email = email;
            _account = account;
        }

        // ==========================================
        // NHỊP 1: API /forgot-password sẽ gọi hàm này
        // ==========================================
        public async Task<bool> GenerateAndSendOtpAsync(string email) 
        {
            var user = await _account.FindByEmailAsync(email); 
            if (user == null) return false;

            int otpCode = Random.Shared.Next(100000, 1000000);
            user.OtpCode = otpCode;
            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(15);
            
            // Gọi thằng Repo lưu sự thay đổi của OTP xuống CSDL
            await _account.SaveChangesAsync(); 

            // Tạm đóng code gửi mail lại tính sau
            // await _email.SendOtpEmailAsync(email, otpCode);

            return true; 
        }

        // ==========================================
        // NHỊP 2: API /reset-password sẽ gọi hàm này
        // ==========================================
        public async Task<bool> VerifyAndResetPasswordAsync(string email, int userOtp, string newPasswordHash)
        {
            var user = await _account.FindByEmailAsync(email);
            if (user == null) return false;

            // Check xem mã OTP khách nhập có đúng và còn hạn không?
            if (user.OtpCode != userOtp || user.OtpExpiryTime < DateTime.UtcNow)
            {
                return false; // Sai mã hoặc hết hạn -> Đuổi về!
            }

            // Tái sử dụng hàm UpdateAccountPasswordAsync thần thánh của em ở đây!
            return await _account.UpdateAccountPasswordAsync(email, newPasswordHash);
        }
    }
}