using Microsoft.EntityFrameworkCore;
using CoffeeShop.Models.Entities.Sales;
using CoffeeShop.DAL.Data;
namespace CoffeeShop.DAL.Repositories
{
    public class OrderDAL {
        private readonly AppDbContext _dbContext;
        public OrderDAL (AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(Guid orderId)
        {
            return await _dbContext.OrderDetails.Where(o => o.OrderId == orderId).ToListAsync();
        }
        public async Task<bool> SaveOrderAsync(Order order, OrderDetail orderDetail)
        {
            //Buoc 1: Nem hoa don vao gio cho
            await _dbContext.Orders.AddAsync(order);
            //Buoc 2: Nem chi tiet vao gio cho
            await _dbContext.OrderDetails.AddAsync(orderDetail);
            //Buoc 3: Chot
            var result = await _dbContext.SaveChangesAsync();
            //Neu result lon hon 0 => Thanh Cong
            return result > 0;
        }
    }
}