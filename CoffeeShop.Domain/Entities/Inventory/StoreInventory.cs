namespace CoffeeShop.Domain.Entities.Inventory
{
    public class StoreInventory 
    {
        public int StoreId { get; set; }
        public int IngredientId { get; set; }
        public int Quantity { get; set; }
    }
}