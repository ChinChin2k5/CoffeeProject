//id, name, description, imageurl, displayorder, isactive 
namespace CoffeeShop.Domain.Entities.Catalog
{
    public class Category 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive {get; set; }
    }
}