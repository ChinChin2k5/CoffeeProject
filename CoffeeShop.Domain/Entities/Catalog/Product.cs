//Id, Name (Nâu đá), Price (35k), Image.
namespace CoffeeShop.Domain.Entities.Catalog
{
    public class Product 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}