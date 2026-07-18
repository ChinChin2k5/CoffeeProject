namespace CoffeeShop.Models.Entities.Catalog
{
    public class Category 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        //Lý do để dấu bằng là để tránh lỗi Null
        // Trong file Category.cs
public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}