using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.BruteForceter;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CoffeeShop.API.Controllers
{
    // Đây là controller phục vụ cho WebApi, trả về một đống JSON
    [ApiController]
    // Định tuyến controller
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public readonly AgainstBruteForce _bruteForceBLL;
        private readonly IConfiguration _configuration;
        public AuthController(AgainstBruteForce bruteForceBLL, IConfiguration configuration)
        {
            _bruteForceBLL = bruteForceBLL;
            _configuration = configuration;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Login([FromBody] LoginRequests login) {
            string result = await _bruteForceBLL.Login(login);
            if (result == "Email không tồn tại !" || result == "Sai mật khẩu!" || result.Contains("khoá")) 
            {
                return BadRequest(new { message = result });
            }
            if (result != "Đăng nhập thành công! Nhả Token ra đây!")
            {
            // Trả về mã 400 Bad Request để JS biết đường mà chặn lại
            return BadRequest(new { message = result });
            }
            string realJwtToken = GenerateJwtToken(login.Email, "Staff");

    var cookieOptions = new CookieOptions
    {
        HttpOnly = true, 
        Secure = false,  
        SameSite = SameSiteMode.Lax, 
        Expires = DateTime.UtcNow.AddDays(1) 
    };

    // Nhét cái VÉ THẬT vào Cookie
    Response.Cookies.Append("accessToken", realJwtToken, cookieOptions);

    return Ok(new { message = "Đăng nhập thành công, token THẬT đã được cất vào két sắt!" });
}
        /*[Authorize(Policy = "AdminOnly")]
        [HttpPost("register-king")]
        public IActionResult Register([FormBody] RegisterNow register) {
            var data = new { message = "...."};
            return Ok(data);
        }*/

        [Authorize(Policy = "ManagerOnly")]
        [HttpGet("manager-data")]
        public IActionResult Manager()
        {
            var data = new { message = "Manager controller" };
            return Ok(data);
        }
        [Authorize(Policy = "StaffOnly")]
        [HttpGet("staff-data")]
        public IActionResult Staff()
        {
            var data = new { message = "Staff execution :v" };
            return Ok(data);
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
    // vẫn còn hạn và đã vượt qua được cửa ải của ông bảo vệ [Authorize].
    // Ta chỉ việc mỉm cười và trả về mã 200 OK cho Frontend.
    
    return Ok(new { message = "Vé còn hạn, mời sếp ở lại chơi!" });
}
private string GenerateJwtToken(string email, string role)
    {
        // Đọc cấu hình từ file appsettings.json
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        // Dùng đúng chìa khóa đó để đúc vé
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(ClaimTypes.Role, role), 
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,       // Không hardcode nữa
            audience: audience,   // Không hardcode nữa
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    [HttpPost("verify-backdoor")]
public IActionResult VerifyBackdoor([FromBody] string inputKey)
{
    var correctKey = _configuration["AdminSettings:BackdoorKey"];
    
    if (inputKey == correctKey)
    {
        return Ok(new { message = "Cửa ải đã mở!" });
    }
    
    return Unauthorized(new { message = "Sai mã bí mật!" });
}
    }
}
