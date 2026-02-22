namespace CoffeeShop.Domain.Entities.Inventory
{
    public class InventoryTransaction 
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int IngredientId { get; set; }
        public int QuantityChange { get; set; }
        public string TransactionType { get; set; }
        public string Reason { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}