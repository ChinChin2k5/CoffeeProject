using Xunit;
using FluentAssertions;
using Moq;
using CoffeeShop.BLL.Interfaces;
using CoffeeShop.DAL.Interfaces;
using CoffeeShop.BLL.Services;
using CoffeeShop.Models.Entities.Auth;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using Microsoft.Extensions.Configuration;

namespace CoffeeShop.Tests;

public class LogInTests
{
    [Theory]
    [InlineData("manager@mycafe.com", "ManagerPass!2", "Manager")]
    [InlineData("staff@mycafe.com", "StaffPass!3", "Staff")]
    public async Task Login_ValidCredentials_ReturnsCorrectRole(string testEmail, string testPassword, string expectedRole)
    {
        var request = new LoginRequests { Email = testEmail, Password = testPassword };
        var mockRepo = new Mock<IUserRepository>();
        var mockTokenService = new Mock<ITokenService>();
        var mockBruteForceService = new Mock<IBruteForceService>();
        var mockConfig = new Mock<IConfiguration>();

        // Dạy Mock
        mockRepo.Setup(repo => repo.GetUserByEmail(testEmail))
                .ReturnsAsync(new User
                {
                    Email = testEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(testPassword),
                    Role = expectedRole
                });

        mockTokenService.Setup(t => t.GenerateJwtToken(It.IsAny<string>(), It.IsAny<string>()))
                        .Returns("fake_jwt_token_for_test");

        var authService = new AuthService(mockRepo.Object, mockTokenService.Object, mockBruteForceService.Object, mockConfig.Object);

        var result = await authService.Login(request);

        result.Should().NotBeNull();
        result.Role.Should().Be(expectedRole);
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task VerifyBackdoor_ValidKey_ReturnsAdminRole()
    {
        // 1. ARRANGE
        string secretKey = "Ma_Bi_Mat_Cua_Tui";

        var mockRepo = new Mock<IUserRepository>();
        var mockTokenService = new Mock<ITokenService>();
        var mockBruteForceService = new Mock<IBruteForceService>();
        var mockConfig = new Mock<IConfiguration>();

        mockConfig.Setup(c => c["AdminSettings:BackdoorKey"]).Returns(secretKey);
        mockRepo.Setup(repo => repo.GetAdminAccount())
                .ReturnsAsync(new User
                {
                    Email = "admin@coffeeshop.com",
                    Role = "Admin",
                    PasswordHash = "BACKDOOR_ONLY_NO_REAL_PASSWORD_ALLOWED_HERE"
                });

        // Setup cho Token Service lỡ hàm Backdoor có gọi tới
        mockTokenService.Setup(t => t.GenerateJwtToken(It.IsAny<string>(), It.IsAny<string>()))
                        .Returns("fake_admin_token");

        var authService = new AuthService(mockRepo.Object, mockTokenService.Object, mockBruteForceService.Object, mockConfig.Object);

        // 2. ACT
        var result = authService.VerifyBackdoor(secretKey);

        // 3. ASSERT
        result.Should().NotBeNull();
        result.Role.Should().Be("Admin");
    }
    [Fact]
    public async Task Login_WrongPassword_ThrowsUnauthorized()
    {
        //1: Arrange
        //Giả sử User trong DB là "aidodeptraisieucapvutru"
        var request = new LoginRequests { Email = "test@cafe.com", Password = "wrong_password" };
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.GetUserByEmail(request.Email))
                .ReturnsAsync(new User { Email = "test@cafe.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("aidodeptraisieucapvutru") });
        var authService = new AuthService(mockRepo.Object, new Mock<ITokenService>().Object, new Mock<IBruteForceService>().Object, new Mock<IConfiguration>().Object);
        //Invoking đòi hỏi trước tiên hàm Login phải thực hiện trước đã, rồi mới thực hiện Fact này
        await authService.Invoking(s => s.Login(request))
                         .Should().ThrowAsync<UnauthorizedAccessException>();
    }
    [Fact]
    public async Task Login_WithinPenaltyTime_ThrowsException()
    {
        // 1. ARRANGE: Dựng hiện trường
        var request = new LoginRequests { Email = "test@cafe.com", Password = "SomePassword123" };

        // Tạo 1 User giả đang bị khóa (Mãn hạn tù vào 15 phút nữa)
        var fakeUser = new User
        {
            Email = request.Email,
            PasswordHash = "hashed_pass",
            LockoutEnd = DateTime.UtcNow.AddMinutes(15)
        };

        var mockRepo = new Mock<IUserRepository>();
        var mockTokenService = new Mock<ITokenService>();
        var mockBruteForceService = new Mock<IBruteForceService>();
        var mockConfig = new Mock<IConfiguration>();

        // Dạy Repo: Khi ai hỏi email này thì đưa fakeUser ra
        mockRepo.Setup(r => r.GetUserByEmail(request.Email))
                .ReturnsAsync(fakeUser);

        // Dạy BruteForceService báo cáo là TÀI KHOẢN ĐANG BỊ KHÓA
        mockBruteForceService.Setup(b => b.IsAccountLocked(fakeUser))
                             .ReturnsAsync(true);

        // Khởi tạo đúng cái AuthService (Lễ tân)
        var authService = new AuthService(mockRepo.Object, mockTokenService.Object, mockBruteForceService.Object, mockConfig.Object);

        // 2 & 3. ACT & ASSERT: Gọi hàm Login và bắt Exception
        await authService.Invoking(s => s.Login(request))
                         .Should().ThrowAsync<UnauthorizedAccessException>()
                         .WithMessage("Tài khoản đã bị khóa*");
    }
}