using Xunit;
using FluentAssertions;
using Moq;
using CoffeeShop.BLL.Interfaces;
using CoffeeShop.BLL.Services;

namespace CoffeeShop.Tests;

public class LogInTests 
{
    [Fact]
    public void Login_ValidCredentials_ReturnsTrue()
    {
        // Dữ liệu đầu vào chuẩn xác mà nhân viên sẽ gõ trên màn hình
        string inputEmail = "manager@mycafe.com";
        string inputPassword = "SuperSecretPassword123!";
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(repo => repo.GetUserByEmail(inputEmail))
                .Returns(new User { Email = inputEmail, Password = inputPassword});
        // Khởi tạo Service chứa logic đăng nhập 
        var authService = new AuthService(mockRepo.Object);
        // Dữ liệu sau đó sẽ được xử lý như sau:
        var result = authService.Login(inputEmail, inputPassword);
        // Dữ liệu kiểm định kết quả
        result.Should().BeTrue();
    }
}

