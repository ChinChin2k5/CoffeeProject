//id, name, description, imageurl, displayorder, isactive 
namespace CoffeeShop.Domain.Entities.Catalog
{
    public class Category 
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string displayorder { get; set; }
        public bool isactive {get; set; }
    }
}