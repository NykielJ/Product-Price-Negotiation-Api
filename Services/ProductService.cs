using ProductPriceNegotiationApi.Data;
using ProductPriceNegotiationApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProductPriceNegotiationApi.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.Name))
                throw new ArgumentException("Product name cannot be empty.");

            if (product.Price <= 0)
                throw new ArgumentException("Price must be greater than 0.");

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetProduct(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
