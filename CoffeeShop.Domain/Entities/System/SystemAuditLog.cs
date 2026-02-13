namespace CoffeeShop.Domain.Entities.System
{
    public class SystemAuditLog 
    {
        public int Id {get; set;}
        public string Action {get; set;}
        public string Descrption {get; set;}
        public int UserId {get; set;}
        public string CreateDate {get; set;}
    }
}