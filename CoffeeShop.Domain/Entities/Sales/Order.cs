namespace CoffeeShop.Domain.Entities.Sales
{
    public class Order 
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public int Status { get; set; }
        public int StaffId { get; set; }
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }
    }
}