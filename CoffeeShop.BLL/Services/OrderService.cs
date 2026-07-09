using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using CoffeeShop.Models.Entities.Sales;
using CoffeeShop.DAL.Repositories;
public class OrderService
{
    private readonly ProductDAL _productDal;
    private readonly OrderDAL _orderDal;
    public OrderService(ProductDAL productDal, OrderDAL orderDal) 
    {
        _productDal = productDal;
        _orderDal = orderDal;
    }
    //Hàm này xử lý thông tin nhận từ order của khách hàng và trả về
    public async Task<CustomerResponse> CreateNewOrderAsync(CustomerRequest request) 
    {
        var productEntity = await _productDal.GetProductByIdAsync(request.ProductId);
        if (productEntity == null) 
        {
            throw new Exception("Quán của tôi không có bán món này !");
        }
        //Mon chinh
        var newOrder = new Order
        {
            Id = Guid.NewGuid(),
            CreateDate = DateTime.UtcNow,
            Status = 1, //1 la Pending
            TotalAmount = productEntity.Price * request.Quantity,
        };
        //Mon phu
        var newOrderDetail = new OrderDetail
        {
            Id = Guid.NewGuid(),
            OrderId = newOrder.Id, //Nhan quan he cha con
            ProductId = productEntity.Id,
            Quantity = request.Quantity,
            Price = productEntity.Price, //Luu gia tai thoi diem ban
        };
        await _orderDal.SaveOrderAsync(newOrder, newOrderDetail);
        var response = new CustomerResponse
        {
            OrderId = newOrder.Id,
            CreateDate = newOrder.CreateDate,
            Status = "Pending",
            TotalAmount = newOrder.TotalAmount,
            Mains = new List<CustomerResponse.MainResponseDTO>
            {
                new CustomerResponse.MainResponseDTO
                {
                    ProductId = productEntity.Id,
                    ProductName = productEntity.Name,
                    Quantity = newOrderDetail.Quantity,
                    Price = newOrderDetail.Price,
                    Toppings = new List<CustomerResponse.MainResponseDTO.ToppingResponseDTO>()
                }
            }
        };
        return response;
    }
}