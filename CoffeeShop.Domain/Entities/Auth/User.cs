//o	Id, Username, PasswordHash, Role (Admin/ Manager (Của các cửa hàng), Staff ).
//FalledLoginAttempts, Lockout End
//StoreId
namespace CoffeeShop.Domain.Entities.Auth
{
    public class User 
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public int FalledLoginAttempts { get; set;}
        public string LockoutEnd { get; set; }
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
