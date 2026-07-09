using Microsoft.EntityFrameworkCore;
using CoffeeShop.Models.Entities.Catalog;
using CoffeeShop.DAL.Data;
namespace CoffeeShop.DAL.Repositories
{
    public class ProductDAL
    {
        private readonly AppDbContext _dbContext;
        public ProductDAL (AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Product> GetProductByIdAsync(int Id)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == Id);
        }
    }
}