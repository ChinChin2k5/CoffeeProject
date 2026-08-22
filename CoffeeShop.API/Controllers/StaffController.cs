using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; // BẮT BUỘC phải có cái này để gọi ClaimTypes
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.Services;
// Nhớ using thư mục chứa InventoryService của đệ vào đây nhé

namespace CoffeeShop.API.StaffController 
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        // 1. CHUYỂN SANG DÙNG INVENTORY SERVICE XỊN
        private readonly InventoryService _inventoryService;
        private readonly ShiftService _shiftService;
        
        public StaffController(InventoryService inventoryService, ShiftService shiftService)
        {
            _inventoryService = inventoryService;
            _shiftService = shiftService;
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
        // File: CoffeeShop.API/Controllers/StaffController.cs
[HttpPost("close-shift")]
        [Authorize(Roles = "Staff,Manager")] // Manager cũng có thể cần chốt ca nếu đứng quầy
        public async Task<IActionResult> CloseShift([FromBody] CloseShiftRequest request)
        {
            try
            {
                // Bóc ID và Tên từ Token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userNameClaim = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
                
                // Giả định đệ có StoreId trong Token, nếu không thì lấy từ request/header
                var storeIdClaim = User.FindFirst("StoreId")?.Value ?? "1"; 

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { message = "Token không hợp lệ!" });

                var report = await _shiftService.CloseShiftAsync(
                    int.Parse(userIdClaim), 
                    int.Parse(storeIdClaim), 
                    userNameClaim, 
                    request
                );

                return Ok(new 
                { 
                    success = true, 
                    message = "Chốt ca thành công!",
                    data = new {
                        systemCalculated = report.SystemCashAmount,
                        actualCount = report.ActualCashAmount,
                        discrepancy = report.Difference
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}