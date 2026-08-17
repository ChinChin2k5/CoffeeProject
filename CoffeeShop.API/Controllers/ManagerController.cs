using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
// Nhớ using BLL Services nếu chưa có nhé đệ

namespace CoffeeShop.API.ManagerController 
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly InventoryService _inventoryService;
        
        // DI tiêm Service vào Controller
        public ManagerController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("manager-data")]
        public IActionResult Manager()
        {
            var data = new { message = "Manager controller" };
            return Ok(data);
        }
    
        // ==================================================
        // ĐÃ KÉO VÀO BÊN TRONG CLASS MANAGERCONTROLLER
        // ==================================================
        [Authorize(Roles = "Manager")] 
        [HttpGet("inventory-transactions")]
        public async Task<IActionResult> GetInventoryTransactions()
        {
            try
            {
                // Controller gọi Service, Service gọi Repo, mượt mà chuẩn 3-Tier!
                var data = await _inventoryService.GetHistoryAsync();
                
                return Ok(new {
                    message = "Lấy lịch sử giao dịch thành công!",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    
    [Authorize(Roles = "Manager")]
        [HttpGet("store-inventory/{storeId}")] // Dùng URL dạng: /api/Manager/store-inventory/1
        public async Task<IActionResult> GetInventoryByStore(int storeId)
        {
            if (storeId <= 0)
            {
                return BadRequest(new { message = "Mã chi nhánh không hợp lệ!" });
            }

            try
            {
                var data = await _inventoryService.GetInventoryByStoreIdAsync(storeId);
                
                return Ok(new {
                    message = $"Lấy tồn kho của chi nhánh {storeId} thành công!",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        } 
    }
}