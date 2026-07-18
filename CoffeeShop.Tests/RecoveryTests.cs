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

public class RecoveryTests
{
    [Fact]
    public async Task VerifyAndResetPassword_ValidOtp_ReturnsTrue()
    {
        string testEmail = "test@cafe.com";
        int validOtp = 123456;
        string newPasswordHash = "new_hashed_password";

        var userInDb = new User
        {
            Email = testEmail,
            OtpCode = validOtp,
            OtpExpiryTime = DateTime.UtcNow.AddMinutes(10) // Còn 10 phút nữa mới hết hạn
        };

        var mockRepo = new Mock<IUserRepository>(); 

        mockRepo.Setup(r => r.FindByEmailAsync(testEmail))
                .ReturnsAsync(userInDb);

        mockRepo.Setup(r => r.UpdateAccountPasswordAsync(testEmail, newPasswordHash))
                .ReturnsAsync(true);

        var recoveryService = new RecoveryService(new Mock<IEmailService>().Object, mockRepo.Object);

        var result = await recoveryService.VerifyAndResetPasswordAsync(testEmail, validOtp, newPasswordHash);

        result.Should().BeTrue();
    }
}