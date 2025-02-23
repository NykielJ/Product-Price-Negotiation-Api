using Microsoft.EntityFrameworkCore;
using ProductPriceNegotiationApi.Models;

namespace ProductPriceNegotiationApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Negotiation> Negotiations { get; set; }
    }
}
