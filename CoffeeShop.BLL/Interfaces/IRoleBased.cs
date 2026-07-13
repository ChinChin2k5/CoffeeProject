using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;

namespace CoffeeShop.BLL.Interfaces
{
    public interface IRoleBased 
    {
        //Đầu vào là hộp Requests, đầu ra là hộp Responses!
        Task<LoginResponses> Login(LoginRequests request);
    }
}