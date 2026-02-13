//Id, StoreName, Address, Hotline.
namespace CoffeeShop.Domain.Entities.Inventory
{
    public class Store 
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string Hotline { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}