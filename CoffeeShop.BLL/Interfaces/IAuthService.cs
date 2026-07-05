namespace CoffeeShop.BLL.Interfaces
{
    // Bản hợp đồng này chỉ ghi đúng 1 dòng: Đứa nào ký, đứa đó phải có hàm Login!
    public interface IAuthService
    {
        bool Login(string email, string password);
    }
}