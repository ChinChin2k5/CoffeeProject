using CoffeeShop.Models.Entities.Auth;

namespace CoffeeShop.BLL.Interfaces
{
    public interface IRecoveryService
    {
        Task<bool> GenerateAndSendOtpAsync(User user);

        Task<bool> VerifyAndResetPasswordAsync(User user);

    }
}