using CoffeeShop.Models.Entities.Auth;
namespace CoffeeShop.BLL.DTOs.Inventory.Requests 
{
    public class LoginRequests
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}