using Xunit;
using FluentAssertions;

namespace CoffeeShop.Tests;

public class LogInTests 
{
    [Fact]
    public void Login_ValidCredentials_ReturnsTrue()
    {
        // Dữ liệu đầu vào chuẩn xác mà nhân viên sẽ gõ trên màn hình
        string inputEmail = "manager@mycafe.com";
        string inputPassword = "SuperSecretPassword123!";
        // Khởi tạo Service chứa logic đăng nhập 
        var authService = new AuthService();
        authService.SeedFakeUserToDatabase(inputEmail, inputPassword, role: "Manager", isActive: true);
        // Dữ liệu sau đó sẽ được xử lý như sau:
        var result = authService.Login(inputEmail, inputPassword);
        // Dữ liệu kiểm định kết quả
        result.Should().BeTrue();
    }
}
public class AuthService 
{
    private string _dbEmail;
    private string _dbPassword;

    public void SeedFakeUserToDatabase(string email, string password, string role, bool isActive)
    {
        _dbEmail = email;
        _dbPassword = password;
    }

    // Đây chính là hàm mà Kỹ sư cần gọi ở bước ACT
    public bool Login(string email, string password)
    {
        if (email == _dbEmail && password == _dbPassword) return true;
        return false;
    }
}
