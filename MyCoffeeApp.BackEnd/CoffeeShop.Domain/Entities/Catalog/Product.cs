//Id, Name (Nâu đá), Price (35k), Image.
namespace CoffeeShop.Domain.Entities.Catalog
{
    public class Product 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        //Khoa Ngoai
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}