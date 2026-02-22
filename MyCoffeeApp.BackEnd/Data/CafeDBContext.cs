using Microsoft.EntityFrameworkCore;
using MyCafeProject.API.Models;

namespace MyCafeProject.API.Data 
{
    public class CafeDBContext : DbContext 
    {
        public CafeDBContext(DbContextOptions<CafeDBContext> options) : base(options) { }
        public DbSet<User>Users {get; set;}
    }
}