using CoffeeShop.DAL.Repositories;
using CoffeeShop.BLL.Interfaces;
using CoffeeShop.BLL.Services;
using CoffeeShop.DAL.Interfaces;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;

namespace CoffeeShop.BLL.Services
{
    public class AuthService : IAuthService 
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;

        public AuthService(IUserRepository userRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        //Trả về ĐÚNG kiểu LoginResponses để khớp với Interface!
        public LoginResponses Login(LoginRequests request)
        {
            // 1. Phải có CHỦ NGỮ (_userRepository) và biến HỨNG DỮ LIỆU (userInDb)
            var userInDb = _userRepository.GetUserByEmail(request.Email);
            
            // 2. Không tìm thấy User trong Database -> Đuổi về
            if (userInDb == null) 
            {
                return null; 
            }
            if (_bruteForceService.IsAccountLocked(userInDb))
            {
                throw new Exception("Tài khoản đã bị khóa do sai quá 5 lần!");
            }

            // 3. So sánh Password truyền vào với PasswordHash lấy từ Database lên
            if (request.Password == userInDb.PasswordHash)
            {
                // 4. Trả về đúng cú pháp tạo Object mới bằng từ khóa 'new'
                return new LoginResponses
                {
                    Role = userInDb.Role, 
                    Token = _tokenService.GenerateJwtToken(request.Email, request.Password) 
                };
            }

            // Sai mật khẩu -> Đuổi về
            return null; 
        }
    }
}