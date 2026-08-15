using Microsoft.AspNetCore.Mvc;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
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
        [HttpPost("confirm-payment")]
        [Authorize(Roles = "Staff,Manager")]
public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest request)
{
    try
    {
        // BƯỚC 3: Móc thẻ ngành ra tự bóc ID và Tên (Y hệt hàm Order)
        var staffIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(staffIdClaim, out int staffId))
        {
            return Unauthorized(new { message = "Token không chứa ID nhân viên hợp lệ hoặc không có quyền!" });
        }

        var staffName = User.FindFirst(ClaimTypes.Name)?.Value ?? "Nhân viên Vô Danh";

        // BƯỚC 4: Lễ tân gọi BLL xử lý, truyền cục data vừa bóc được từ Token xuống
        var response = await _orderService.ConfirmPaymentAsync(request, staffId, staffName);
        
        return Ok(response);
    }
    catch (Exception ex)
    {
        if (ex.Message == "Order_Not_Found")
        {
            return NotFound(new { error = "Không tìm thấy hoá đơn này!" });
        }
        if (ex.Message == "Order_Already_Paid")
        {
            return BadRequest(new { error = "Hoá đơn này đã được thanh toán từ trước!" });
        }
        
        return StatusCode(500, new { error = "Lỗi hệ thống: " + ex.Message });
    }
}
[HttpPost("{orderId}/cancel")]
[Authorize(Roles = "Staff,Manager")] // Kẹp Token vào, cho phép cả Staff và Manager hủy đơn
public async Task<IActionResult> CancelOrder(Guid orderId)
{
    try
    {
        // 1. Tận dụng tuyệt chiêu bóc Token xịn sò của đệ
        var staffIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(staffIdClaim, out int staffId))
        {
            return Unauthorized(new { message = "Token không chứa ID nhân viên hợp lệ hoặc không có quyền!" });
        }

        // 2. Gọi BLL xử lý nghiệp vụ hủy đơn và nhả kho
        bool result = await _orderService.CancelOrderAsync(orderId, staffId);
        
        if (result)
        {
            return Ok(new 
            { 
                success = true, 
                message = "Hủy đơn hàng và hoàn trả nguyên liệu thành công!" 
            });
        }
        
        return BadRequest(new { success = false, message = "Có lỗi xảy ra, không thể hủy đơn hàng." });
    }
    catch (Exception ex)
    {
        // Chỉ những lỗi do chính tay mình chủ động ném ra (rào chắn) mới cho hiển thị lên Frontend
        if (ex.Message.Contains("Cảnh báo bảo mật") || ex.Message.Contains("Không tìm thấy"))
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        
        // Sẵn tiện ghi log lại lỗi thật (ex) vào file hoặc Console để anh em Dev tự đọc.
        Console.WriteLine($"[LỖI HỆ THỐNG - CancelOrder] {ex.ToString()}");
        return StatusCode(500, new { success = false, message = "Đã có lỗi hệ thống xảy ra, vui lòng thử lại sau!" });
    }
}
[HttpGet]
[Authorize(Roles = "Manager,Staff")] // Cho phép Quản lý và Nhân viên xem
public async Task<IActionResult> GetAllOrders()
{
    try
    {
        // Gọi Service lấy danh sách
        var orders = await _orderService.GetOrderSummariesAsync();
        
        // Trả về JSON bọc trong biến 'data' (để khớp với code JavaScript result.data)
        return Ok(new { data = orders });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
    }
}
    }
}