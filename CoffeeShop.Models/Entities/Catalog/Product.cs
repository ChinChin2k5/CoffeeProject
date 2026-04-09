namespace CoffeeShop.Models.Entities.Catalog
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