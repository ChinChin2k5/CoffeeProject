using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.BruteForceter;
using System;

namespace CoffeeShop.API.Controllers
{
    // Đây là controller phục vụ cho WebApi, trả về một đống JSON
    [ApiController]
    // Định tuyến controller
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public readonly AgainstBruteForce _bruteForceBLL;
        public AuthController(AgainstBruteForce bruteForceBLL)
        {
            _bruteForceBLL = bruteForceBLL;
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
            // 1. Tạo hình hài cho cái két sắt Cookie
    var cookieOptions = new CookieOptions
    {
        HttpOnly = true, 
        Secure = false,  
        SameSite = SameSiteMode.Lax, 
        Expires = DateTime.UtcNow.AddDays(1) 
    };

    // 2. Nhét Token vào Cookie và gắn vào thư trả về
    Response.Cookies.Append("accessToken", result, cookieOptions);

    // 3. Trả JSON về cho Frontend, không cần gửi Token nữa
    return Ok(new { message = "Đăng nhập thành công, token đã được cất vào két sắt!" });
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
    }
}
