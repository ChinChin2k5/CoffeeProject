using CoffeeShop.DAL.Repositories;
using CoffeeShop.BLL.Interfaces;
using CoffeeShop.BLL.Services;
using CoffeeShop.DAL.Interfaces;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using Microsoft.Extensions.Configuration;

namespace CoffeeShop.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IBruteForceService _bruteForceService;

        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, ITokenService tokenService, IBruteForceService bruteForceService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _bruteForceService = bruteForceService;
            _configuration = configuration;
        }

        //Trả về ĐÚNG kiểu LoginResponses để khớp với Interface!
        public async Task<LoginResponses> Login(LoginRequests request)
        {
            // 1. Phải có CHỦ NGỮ (_userRepository) và biến HỨNG DỮ LIỆU (userInDb)
            var userInDb = await _userRepository.GetUserByEmail(request.Email);

            // 2. Không tìm thấy User trong Database -> Đuổi về
            if (userInDb == null)
            {
                return null;
            }
            if (await _bruteForceService.IsAccountLocked(userInDb.Email))
            {
                throw new Exception("Tài khoản đã bị khóa do sai quá 5 lần!");
            }
            bool isMatch = BCrypt.Net.BCrypt.Verify(request.Password, userInDb.PasswordHash);
            // 3. So sánh Password truyền vào với PasswordHash lấy từ Database lên
            if (!isMatch)
            {
                await _bruteForceService.CountBruteForce(userInDb.Email);
                throw new UnauthorizedAccessException("Sai mật khẩu!");
            }
            await _bruteForceService.ResetFalledAttemptAsync(userInDb.Email);
            string realToken = _tokenService.GenerateJwtToken(userInDb.Email, userInDb.Role);
            return new LoginResponses
            {
                Role = userInDb.Role,
                Token = realToken
            };
        }
        // Trong AuthService.cs (Nhớ khai báo cả ở IAuthService nhé)
        public LoginResponses VerifyBackdoor(string inputKey)
        {
            // Lấy chìa khóa từ két sắt (appsettings.json)
            var correctKey = _configuration["AdminSettings:BackdoorKey"];

            if (inputKey != correctKey)
            {
                return null; // Sai key thì trả về null (hoặc ném Exception)
            }

            // 1. Nặn Token thật cho Admin quản trị hệ thống
            string realJwtToken = _tokenService.GenerateJwtToken("admin@khoahoc.vn", "Admin");

            // 2. Đóng gói trả về DTO
            return new LoginResponses
            {
                Role = "Admin",
                Token = realJwtToken
            };
        }
    }
}