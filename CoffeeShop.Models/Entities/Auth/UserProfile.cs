namespace CoffeeShop.Models.Entities.Auth
{
    public class UserProfile  
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public virtual User User { get; set; }
    }
}