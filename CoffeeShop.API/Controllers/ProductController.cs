using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.DAL.Repositories; 
using Microsoft.AspNetCore.Authorization;

namespace CoffeeShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] 
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _productRepo;

        public ProductController(ProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepo.GetAllProductsWithCategoryAsync();
            // 2. Định hình lại gói hàng (Flat JSON) để ném ra cho Frontend 
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