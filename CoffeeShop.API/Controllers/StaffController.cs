using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; // BẮT BUỘC phải có cái này để gọi ClaimTypes
using CoffeeShop.BLL.DTOs.Inventory.Requests;
// Nhớ using thư mục chứa InventoryService của đệ vào đây nhé

namespace CoffeeShop.API.StaffController 
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        // 1. CHUYỂN SANG DÙNG INVENTORY SERVICE XỊN
        private readonly InventoryService _inventoryService;
        
        public StaffController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
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
        // 2. Đổi StaffRequest thành ImportInventoryRequest (cái DTO đã xóa storeId)
        public async Task<IActionResult> ImportInventory([FromBody] ImportInventoryRequest request) 
        {
            if (request == null || request.ItemId <= 0 || request.QuantityToAdd <= 0)
            {
                return BadRequest(new {message = "Các thông tin không hợp lệ"});
            }

            // ================= CHỖ FIX LỖI 401 ĐÂY =================
            // Móc thẻ ngành bằng NameIdentifier thay vì "Id"
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new { message = "Không tìm thấy thẻ ngành hợp lệ!" }); 
            }
            
            int realStaffId = int.Parse(userIdClaim); // Ép ra số nguyên

            try 
            {
                // 3. Gọi hàm mới ImportInventoryAsync của InventoryService
                var responseDto = await _inventoryService.ImportInventoryAsync(realStaffId, request);
                
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