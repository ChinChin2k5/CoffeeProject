using CoffeeShop.Models.Entities.Auth;
namespace CoffeeShop.BLL.DTOs.Inventory.Requests 
{
    public class ForgotPasswordRequest 
    {
        public string Email { get; set; }
    }
}