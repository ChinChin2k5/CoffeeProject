using Microsoft.AspNetCore.Mvc;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using CoffeeShop.BLL;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CoffeeShop.Models.Entities.Enums;


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
public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest request)
{
    try
    {
        // Bước 1: Lễ tân gọi BLL xử lý
        var response = await _orderService.ConfirmPaymentAsync(request);
        
        // Bước 6: Nhẹ nhàng return Ok
        return Ok(response);
    }
    catch (Exception ex)
    {
        // Tạm thời dùng try-catch để map lỗi từ Bước 3 ra HTTP Status Code
        if (ex.Message == "Order_Not_Found")
        {
            return NotFound(new { error = "Không tìm thấy hoá đơn này!" });
        }
        if (ex.Message == "Order_Already_Paid")
        {
            return BadRequest(new { error = "Hoá đơn này đã được thanh toán từ trước!" });
        }
        
        // Cứu cánh cuối cùng cho lỗi không lường trước
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
[HttpGet("test-race-condition")]
[Authorize(Roles = "Staff")] // Chơi khắt khe y như hàm gốc luôn
public async Task<IActionResult> TestRaceCondition()
{
    // Đệ TỰ GÕ lại cổng nhé, cấm copy!
    string apiUrl = "http://localhost:5059/api/Order/order"; 
    
    var httpClient = new HttpClient();

    // ======================================================
    // 1. TỰ ĐỘNG LẤY THẺ TỪ SWAGGER VÀ TẨY TRẦN UNICODE
    // ======================================================
    var authHeader = Request.Headers["Authorization"].ToString();
    if (!string.IsNullOrEmpty(authHeader))
    {
        var rawToken = authHeader.Replace("Bearer", "").Trim();
        var cleanToken = new string(rawToken.Where(c => c >= 32 && c <= 126).ToArray());
        httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cleanToken);
    }

    var tasks = new List<Task<HttpResponseMessage>>();
    Console.WriteLine("Bắt đầu khởi tạo 10 request đặt hàng cùng lúc...");

    for (int i = 0; i < 10; i++)
    {
        // 2. TẠO PAYLOAD BẰNG OBJECT (Miễn nhiễm ma Unicode)
        var payload = new 
        {
            customerId = 1,
            storeId = 1,
            paymentMethod = "Tiền mặt",
            items = new[] 
            {
                new 
                {
                    productId = 5, 
                    quantity = 1,
                    toppings = new[] 
                    {
                        new { productId = 13, quantity = 1 }
                    }
                }
            }
        };

        var content = JsonContent.Create(payload);
        tasks.Add(httpClient.PostAsync(apiUrl, content));
    }

    var results = await Task.WhenAll(tasks);

    int successCount = results.Count(r => r.IsSuccessStatusCode);
    int failCount = results.Count(r => !r.IsSuccessStatusCode);

    var firstFailed = results.FirstOrDefault(r => !r.IsSuccessStatusCode);
    string reason = "Không có lỗi (10 đơn vào trót lọt!)";
    if (firstFailed != null)
    {
        reason = $"Mã lỗi: {firstFailed.StatusCode} - Chi tiết: {await firstFailed.Content.ReadAsStringAsync()}";
    }

    return Ok(new { 
        Message = "Test Race Condition Hoàn Tất!", 
        DonHangThanhCong = successCount, 
        DonHangThatBai = failCount,
        LyDoThatBai = reason
    });
}
    }
}