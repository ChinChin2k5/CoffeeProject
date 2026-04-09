namespace CoffeeShop.Models.Entities.Sales
{
    public class OrderDetail
    {
        public Guid OrderId {get; set;}
        public int ProductId {get; set;}
        public int Quantity {get; set;}
        public decimal Price {get; set;}
        public Guid? ParentOrderDetailId {get; set;}
    }
}