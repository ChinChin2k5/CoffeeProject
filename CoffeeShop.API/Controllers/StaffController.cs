using Microsoft.AspNetCore.Mvc;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using Microsoft.AspNetCore.Authorization;
namespace CoffeeShop.API.StaffController 
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly StaffService _staffService;
        public StaffController(StaffService staffService)
        {
            _staffService = staffService;
        }
        [Authorize(Roles = "Staff")]
        [HttpGet("staff-data")]
        public IActionResult Staff()
        {
            var data = new { message = "Staff execution :v" };
            return Ok(data);
        }
        [HttpPost("import-inventory")]
[Authorize(Roles = "Staff, Manager")]
public async Task<IActionResult> ImportInventory([FromBody] StaffRequest request) // TUYỆT ĐỐI KHÔNG để staffId ở đây!
{
    if (request == null || request.ItemId <= 0 || request.QuantityToAdd <= 0)
    {
        return BadRequest(new {message = "Các thông tin không hợp lệ"});
    }

    // 1. Móc thẻ ngành JWT ra để lấy ID xịn (Chống Hacker)
    var userIdClaim = User.FindFirst("Id")?.Value; 
    if (string.IsNullOrEmpty(userIdClaim))
    {
        return Unauthorized(new { message = "Không tìm thấy thẻ ngành hợp lệ!" }); 
    }
    
    int realStaffId = int.Parse(userIdClaim); // Ép ra số nguyên

    try 
    {
        // 2. Truyền đúng thứ tự (ID trước, Request sau) và KHÔNG CÓ CHỮ "int"
        var responseDto = await _staffService.CreateNewImportAsync(realStaffId, request);
        
        return Ok(new {
            message = "Nhập kho thành công rực rỡ!",
            data = responseDto
        });
    } 
    catch (Exception ex)
    {
        return BadRequest(new {message = ex.Message});
    }
}
    }
}