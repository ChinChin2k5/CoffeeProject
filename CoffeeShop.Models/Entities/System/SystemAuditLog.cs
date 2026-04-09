namespace CoffeeShop.Models.Entities.System
{
    public class SystemAuditLog 
    {
        public int Id {get; set;}
        public string Action {get; set;}
        public string Description {get; set;}
        public int UserId {get; set;}
        public DateTime CreateDate {get; set;}
    }
}