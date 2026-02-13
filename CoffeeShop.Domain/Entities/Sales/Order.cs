namespace CoffeeShop.Domain.Entities.Sales
{
    public class Order 
    {
        public Guid Id { get; set; }
        public string CreateDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public int Status { get; set; }
        public string StaffId { get; set; }
        public string StoreId { get; set; }
    }
}