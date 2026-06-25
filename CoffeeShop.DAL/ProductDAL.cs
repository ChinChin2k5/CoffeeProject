using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Models.Entities.Catalog;
namespace CoffeeShop.DAL
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