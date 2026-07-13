/*using Xunit;
using FluentAssertions;
using Moq;
using CoffeeShop.BLL.Interfaces;
using CoffeeShop.BLL.Services;

namespace CoffeeShop.Tests;

public class RoleManager
{
    [Fact]
    public void Login_WithManagerAccount_ReturnsManagerRole() 
    {
        string inputEmail = "manager@mycafe.com";
        string inputPassword = "SuperPassword123!";
        string RoleManager = "Manager";
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(repo => repo.GetUserByEmail(inputEmail))
                .Returns(new User 
                {
                    Email = inputEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(inputPassword),
                    RoleManager = expectedRole
                });
        // Khởi tạo Service chứa logic đăng nhập 
        var authService = new AuthService(mockRepo.Object);
        // Dữ liệu sau đó sẽ được xử lý như sau:
        var result = authService.Login(inputEmail, inputPassword);
        // Dữ liệu kiểm định kết quả
        result.Should().NotBeNull();
        result.Role.Should().Be(expectedRole);

    }
}*/