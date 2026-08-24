using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService; 

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var menu = await _productService.GetMenuAsync();
            return Ok(new
            {
                message = "Tải menu thành công!",
                data = menu
            });
        }
    }
}