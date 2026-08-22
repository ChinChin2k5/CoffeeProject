using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.Interfaces;
using CoffeeShop.BLL.Services;

namespace CoffeeShop.API.Controllers
{
    // Đây là controller phục vụ cho WebApi, trả về một đống JSON
    [ApiController]
    // Định tuyến controller
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly BruteForceService _bruteForceService;
        private readonly IConfiguration _configuration;
        private readonly OrderService _orderService;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly PasswordHasher _passwordHasher;
        public AuthController(BruteForceService bruteForceService,
        IConfiguration configuration,
        OrderService orderService,
        IAuthService authService,
        ITokenService tokenService,
        PasswordHasher passwordHasher)
        {
            _bruteForceService = bruteForceService;
            _configuration = configuration;
            _orderService = orderService;
            _authService = authService;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Login([FromBody] LoginRequests login)
        {
            try
            {
                var result = await _authService.Login(login);
                if (result == null)
                {
                    return BadRequest(new { message = "Email này không tồn tại trong hệ thống!" });
                }
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,  //Khi nào chạy production thì nhớ để true
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddDays(1)
                };

                // Nhét cái VÉ THẬT vào Cookie
                Response.Cookies.Append("accessToken", result.Token, cookieOptions);

                return Ok(new
                {
                    message = "Đăng nhập thành công, token THẬT đã được cất vào két sắt!",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã có lỗi hệ thống xảy ra, vui lòng thử lại sau!" });
            }
        }
        [HttpPost("logout")]
        [AllowAnonymous] // Ai cũng có quyền bấm đăng xuất
        public IActionResult Logout()
        {
            // Lệnh cho trình duyệt thủ tiêu cái bánh quy mang tên "accessToken"
            Response.Cookies.Delete("accessToken");
            return Ok(new { message = "Đăng xuất thành công, đã thu hồi lệnh bài!" });
        }
        [HttpGet("me")]
        [Authorize] // Tấm khiên cực kỳ quan trọng! Cấm kẻ không có vé (Cookie) được đi qua.
        public IActionResult GetMe()
        {
            // Nếu user chọc được vào đến dòng code này, chứng tỏ Token/Cookie của họ 

            return Ok(new { message = "Vé còn hạn, mời sếp ở lại chơi!" });
        }
    }
}
