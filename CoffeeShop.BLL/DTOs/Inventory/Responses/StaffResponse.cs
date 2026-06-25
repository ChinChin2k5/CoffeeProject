namespace CoffeeShop.BLL.DTOs.Inventory.Responses
{
    public class StaffResponse
    {
        public int StaffId { get; set; }
        public string ItemName { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int NewStockQuantity { get; set; }
    }
}