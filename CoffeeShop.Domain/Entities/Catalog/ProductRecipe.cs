//ProductId, IngredientId, QuantityNeeded (Số lượng cần dùng).
namespace CoffeeShop.Domain.Entities.Catalog
{
    public class ProductRecipe 
    {
        public int ProductId { get; set; }
        public int IngredientId { get; set; }
        public int QuantityNeeded { get; set; }
    }
}