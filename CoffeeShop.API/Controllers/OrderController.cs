using Microsoft.AspNetCore.Mvc;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using CoffeeShop.BLL;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CoffeeShop.API.OrderController 
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("order")]
        [Authorize(Roles = "Staff")] // Bắt buộc phải kẹp Token vào mới cho chạy
        public async Task<IActionResult> CustomerOrder([FromBody] CreateOrderRequest request) 
        {
            if (request == null)
            {
                return BadRequest(new { message = "Mày đã order đâu" });
            }
            try 
            {
                // Hứng Token do Auth nhả ra để đảm bảo tính nhất quán
                var staffIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                // 2. Chuyển thành số và kiểm tra. 
// Chú ý: Cụm "out int staffId" sẽ tự động sinh ra biến staffId để dùng ở các dòng bên dưới!
if (!int.TryParse(staffIdClaim, out int staffId))
{
    return Unauthorized(new { message = "Token không chứa ID nhân viên hợp lệ hoặc không có quyền!" });
}
// 2. BÓC LUÔN STAFF NAME TỪ TOKEN
                // Dùng ClaimTypes.Name, nếu không có thì fallback về "Nhân viên Vô Danh" để bill không bị null
                var staffName = User.FindFirst(ClaimTypes.Name)?.Value ?? "Nhân viên Vô Danh";
                
                // 2. Ném hộp xuống tầng BLL (OrderService) để nó xử lý DB và tính tiền.
                // Hứng lại cái hóa đơn từ BLL trả lên.
                var responseDto = await _orderService.CreateNewOrderAsync(request, staffId, staffName);
                // 3. Trả về mã 201 kèm cái hóa đơn cho Frontend in ra bill
                return StatusCode(201, new {
                    message = "Ok rồi nhé, bill của bro đây",
                    data = responseDto
                });
            } catch (Exception ex)
            {
                // Nếu tầng BLL check DB thấy món nước không tồn tại, nó ném lỗi lên đây
        return BadRequest(new { message = ex.Message });
            }
        }
    }
}