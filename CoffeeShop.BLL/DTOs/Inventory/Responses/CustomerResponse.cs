namespace CoffeeShop.BLL.DTOs.Inventory.Responses
{
    public class CustomerResponse 
    {
        public Guid OrderId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<MainResponseDTO> Mains { get; set; } = new List<MainResponseDTO>();
        public class MainResponseDTO 
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public List<ToppingResponseDTO> Toppings { get; set; } = new List<ToppingResponseDTO>();
            public class ToppingResponseDTO 
            {
                public int ProductId { get; set; }
                public string ProductName { get; set; }
                public int Quantity { get; set; }
                public decimal Price { get; set; }
            }
        }
    }
}