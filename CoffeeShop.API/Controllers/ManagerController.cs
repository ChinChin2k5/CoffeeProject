using Microsoft.AspNetCore.Mvc;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using CoffeeShop.BLL;
using Microsoft.AspNetCore.Authorization;
namespace CoffeeShop.API.ManagerController 
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        [Authorize(Roles = "Manager")]
        [HttpGet("manager-data")]
        public IActionResult Manager()
        {
            var data = new { message = "Manager controller" };
            return Ok(data);
        }
    }
}