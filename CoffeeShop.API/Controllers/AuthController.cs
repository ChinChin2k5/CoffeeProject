// Đây là controller phục vụ cho WebApi, trả về một đống JSON
[ApiController]
// Định tuyến controller
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("admin-data")]
    public IActionResult GetAdminData() {
        var data = new { message = "Admin lover", ServerTime = DateTime.Now};
        return Ok(data);
    }
    [Authorize(Policy = "ManagerOnly")]
    [HttpGet("manager-data")]
    public IActionResult Manager() {
        var data = new { message = "Manager controller"};
        return Ok(data);
    }
    [Authorize(Policy = "StaffOnly")]
    [HttpGet("staff-data")]
    public IActionResult Staff() {
        var data = new { message = "Staff execution :v"};
        return Ok(data);
    }
}
