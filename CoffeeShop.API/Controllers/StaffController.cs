using Microsoft.AspNetCore.Mvc;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using CoffeeShop.BLL;
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
        public async Task<IActionResult> ImportInventory([FromBody] StaffRequest request)
        {
            if (request == null || request.StaffId <= 0 || request.ItemId <= 0 || request.QuantityToAdd <= 0)
            {
                return BadRequest(new {message = "Các thông tin không hợp lệ"});
            }
            try 
        {
            var responseDto = await _staffService.CreateNewImportAsync(request);
            return Ok(new {
                message = "Nhập kho thành công rực rỡ!",
                data = responseDto
            });
        } catch (Exception ex)
        {
            return BadRequest(new {message = ex.Message});
        }
        }
    }
}