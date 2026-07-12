using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using Microsoft.IdentityModel.Tokens;
using CoffeeShop.BLL.Interfaces;
using CoffeeShop.DAL.Interfaces;
using CoffeeShop.DAL.Data;
using CoffeeShop.DAL.Repositories;
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
        private readonly RecoveryService _recoveryService;
        private readonly OrderService _orderService;
        private readonly IAuthService _authService;
        private readonly TokenService _tokenService;
        private readonly PasswordHasher _passwordHasher;
        public AuthController(BruteForceService bruteForceService, 
        IConfiguration configuration, 
        RecoveryService recoveryService, 
        OrderService orderService, 
        IAuthService authService, 
        TokenService tokenService,
        PasswordHasher passwordHasher)
        {
            _bruteForceService = bruteForceService;
            _configuration = configuration;
            _recoveryService = recoveryService;
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
                //Nặn token thật
                string realJwtToken = _tokenService.GenerateJwtToken(login.Email, result.Role);
                result.Token = realJwtToken;
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,  //Khi nào chạy production thì nhớ để true
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddDays(1)
                };

                // Nhét cái VÉ THẬT vào Cookie
                Response.Cookies.Append("accessToken", realJwtToken, cookieOptions);

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
        /*[Authorize(Policy = "AdminOnly")]
        [HttpPost("register-king")]
        public IActionResult Register([FormBody] RegisterNow register) {
            var data = new { message = "...."};
            return Ok(data);
        }*/
        // Hàm logout này tí nữa mình sẽ xử lý
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
            // vẫn còn hạn và đã vượt qua được cửa ải của ông bảo vệ [Authorize].
            // Ta chỉ việc mỉm cười và trả về mã 200 OK cho Frontend.

            return Ok(new { message = "Vé còn hạn, mời sếp ở lại chơi!" });
        }
        [HttpPost("verify-backdoor")]
        public IActionResult VerifyBackdoor([FromBody] string inputKey)
        {
            var correctKey = _configuration["AdminSettings:BackdoorKey"];

            if (inputKey == correctKey)
    {
        // 1. Nặn Token thật cho Admin (Ép cứng email của ông Admin thủy tổ vào đây)
        string realJwtToken = _tokenService.GenerateJwtToken("admin@coffeeshop.com", "Admin");

        // 2. Nhét VÉ vào két sắt Cookie
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // Khi lên Prod nhớ đổi thành true
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(1)
        };
        Response.Cookies.Append("accessToken", realJwtToken, cookieOptions);

        // 3. Phải trả thêm cái Role về cho Frontend để nó cất vào Balo
        return Ok(new 
        { 
            message = "Cửa ải đã mở!",
            data = new { role = "Admin" } // <-- Quan trọng!
        });
    }

            return Unauthorized(new { message = "Sai mã bí mật!" });
        }
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> SendOtp([FromBody] ForgotPasswordRequest request)
        {
            // Gọi Service xử lý bất đồng bộ, dùng await để đợi kết quả true/false thật
            bool result = await _recoveryService.GenerateAndSendOtpAsync(request.Email);

            // Nếu Service lắc đầu (Email không tồn tại)
            if (!result)
            {
                return BadRequest(new { message = "Email này không tồn tại trong hệ thống!" });
            }

            return Ok(new { message = "Mã OTP đã được gửi, vui lòng kiểm tra hòm thư của bạn!" });
        }

        // ==========================================
        // NHỊP 2: XÁC THỰC OTP VÀ ĐỔI MẬT KHẨU
        // ==========================================
        [HttpPost("reset-password")]
        [AllowAnonymous]
        [EnableRateLimiting("fixed")]

        public async Task<IActionResult> RecoveryPassword([FromBody] ResetPasswordRequest request)
        {
            // Lưu ý: Chỗ này trước khi gọi Service, em nhớ dùng BCrypt để băm mật khẩu mới ra nhé!
            string newPasswordHash = _passwordHasher.Hash(request.NewPassword);

            // Gửi dữ liệu xuống Nhịp 2 của Service để kiểm tra chéo với DB
            bool result = await _recoveryService.VerifyAndResetPasswordAsync(request.Email, request.OtpCode, newPasswordHash);

            // Nếu Service báo sai mã OTP hoặc mã đã hết hạn
            if (!result)
            {
                return BadRequest(new { message = "Mã OTP sai hoặc đã hết hạn sử dụng!" });
            }

            return Ok(new { message = "Đổi mật khẩu thành công! Mời sếp đăng nhập lại." });
        }
    }
}
