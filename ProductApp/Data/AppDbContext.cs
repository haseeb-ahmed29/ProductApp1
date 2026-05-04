using Microsoft.EntityFrameworkCore;
using ProductApp.Models;

namespace ProductApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}