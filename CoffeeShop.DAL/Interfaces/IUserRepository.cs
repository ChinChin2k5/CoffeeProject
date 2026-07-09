using CoffeeShop.Models.Entities.Auth;
namespace CoffeeShop.DAL.Interfaces
{
    public interface IUserRepository
    {
        //Có nhiệm vụ lấy email và trả về thông tin người dùng dùng (nếu có)
        User GetUserByEmail(string email);
    }
}