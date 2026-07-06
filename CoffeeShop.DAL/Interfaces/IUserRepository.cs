using CoffeeShop.Models.Entities.User.Auth;
namespace CoffeeShop.BLL.Interfaces
{
    public interface IUserRepository
    {
        //Có nhiệm vụ lấy email và trả về thông tin người dùng dùng (nếu có)
        User GetUserByEmail(string email);
    }
}