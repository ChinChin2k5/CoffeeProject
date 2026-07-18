namespace CoffeeShop.BLL.DTOs.Inventory.Requests
{
    public class CustomerRequest
    {
        public int CustomerId { get; set; }
        // Đây là ly nước chính
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string PaymentMethod { get; set; }

        // Đây là danh sách topping chui thẳng vào trong ly nước
        public List<ToppingRequestDTO> Toppings { get; set; } = new List<ToppingRequestDTO>();

    }
    public class ToppingRequestDTO
    {
        // Đây là thông tin topping
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}