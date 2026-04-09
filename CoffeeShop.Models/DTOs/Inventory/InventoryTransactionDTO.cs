namespace CoffeeShop.Models.DTOs.Inventory
{
    public class InventoryTransactionDTO 
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}