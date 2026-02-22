using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.API.Controllers
{
    [Route("api/[controller]")] // Tự động biến thành /api/auth
    [ApiController]
    public class AuthController : ControllerBase
    {
        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
        
        [HttpPost("login")] // Đường dẫn đầy đủ sẽ là: POST /api/auth/login
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "admin" && request.Password == "123456")
            {
                return Ok(new { message = "Đăng nhập thành công !", role = "Admin"});
            }
            return Unauthorized(new {message = "Sai tài khoản hoặc mật khẩu !" });
        }
    }
}
