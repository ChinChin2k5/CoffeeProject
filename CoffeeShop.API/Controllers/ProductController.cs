using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.DAL.Repositories; // Chỉnh lại theo namespace DB Context của sếp
using Microsoft.AspNetCore.Authorization;

namespace CoffeeShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] // Tạm thời comment cái này lại để test cho dễ, lấy được data lên hình rồi hẵng bật lại bảo vệ sau
    public class ProductController : ControllerBase
    {
        private readonly ProductDAL _productDAL;

        public ProductController(ProductDAL productDAL)
        {
            _productDAL = productDAL;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productDAL.GetAllProductsWithCategoryAsync();
            // 2. Định hình lại gói hàng (Flat JSON) để ném ra cho Frontend dễ nuốt
            var result = products.Select(p => new {
                id = p.Id,
                name = p.Name,
                price = p.Price,
                image = p.Image,
                categoryName = p.Category != null ? p.Category.Name : "Chưa phân loại"
            });
            return Ok(new { 
                message = "Tải menu thành công!",
                data = result 
            });
        }
    }
}