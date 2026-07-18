using CoffeeShop.Models.Entities.Auth;

namespace CoffeeShop.BLL.Interfaces
{
    public interface IBruteForceService
    {
        Task<bool> IsAccountLocked(User user);

        Task CountBruteForce(User user);

        Task ResetFalledAttemptAsync(User user);
    }
}