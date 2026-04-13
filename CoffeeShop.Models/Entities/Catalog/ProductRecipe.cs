namespace CoffeeShop.Models.Entities.Catalog
{
    public class ProductRecipe 
    {
        public int ProductId { get; set; }
        public int ItemId { get; set; }
        public decimal QuantityNeeded { get; set; }
    }
}