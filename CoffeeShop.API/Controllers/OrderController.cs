using Microsoft.AspNetCore.Mvc;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using System;
using CoffeeShop.BLL;
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
        public async Task<IActionResult> CustomerOrder([FromBody] CustomerRequest request) 
        {
            if (request == null || request.ProductId <= 0 || request.Quantity <= 0)
            {
                return BadRequest(new { message = "Mày đã order đâu hoặc order láo hả thằng loz" });
            }
            try 
            {
                // 2. Ném hộp xuống tầng BLL (OrderService) để nó xử lý DB và tính tiền.
                // Hứng lại cái hóa đơn (CustomerResponse) từ BLL trả lên.
                var responseDto = await _orderService.CreateNewOrderAsync(request);
                // 3. Trả về mã 201 kèm cái hóa đơn cho Frontend in ra bill
                return StatusCode(201, new {
                    message = "Ok rồi nhé thằng loz, bill của mày đây",
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