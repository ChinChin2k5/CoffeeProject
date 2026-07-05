using System;
using CoffeeShop.BLL.Interfaces;
// Nhớ using các DTO nếu em dùng LoginRequests và LoginResponses

namespace CoffeeShop.BLL.Services
{
    // Đừng quên ký Hợp đồng IAuthService nhé
    public class AuthService : IAuthService 
    {
        private readonly IUserRepository _userRepository;

        // 1. Giải quyết lỗi CS1729: Mở phễu (Constructor) để nhận cái mockRepo từ bài Test tiêm vào
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // 2. Giải quyết lỗi CS1061: Khai báo hàm Login
        // Chú ý: Đầu vào và đầu ra phải khớp với DTOs mà em đã định nghĩa
        public bool Login(string email, string password) 
        {
            // Tạm thời ném ra một cái lỗi chưa implement để xí chỗ.
            // Lát nữa bài Test gọi vào đây sẽ bị tạch, lúc đó mình mới bắt đầu viết logic thật!
            throw new NotImplementedException("Đại ca từ từ, em chưa code xong logic bên trong!");
        }
    }
}