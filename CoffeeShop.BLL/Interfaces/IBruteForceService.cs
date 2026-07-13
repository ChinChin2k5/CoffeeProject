using CoffeeShop.Models.Entities.Auth;

namespace CoffeeShop.BLL.Interfaces
{
    public interface IBruteForceService
    {
        Task<bool> IsAccountLocked(string email);

        Task CountBruteForce(string email);

        Task ResetFalledAttemptAsync(string email);
    }
}