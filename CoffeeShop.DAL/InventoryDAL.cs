/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Models.Entities.Inventory;
namespace CoffeeShop.DAL
{
    public class InventoryDAL
    {
        private readonly AppDbContext _dbContext;
        public InventoryDAL (AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //Khi có khách Order thì trừ kho đi
        public async Task<StoreInventory> DeductStockForOrderAsync(int Quantity)
        {
            
        }
    }
}*/