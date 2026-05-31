namespace CoffeeShop.Models.Entities.Catalog
{
    public class InventoryItem 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; }
        public virtual ICollection<ProductRecipe> ProductRecipes { get; set; }
    }
}