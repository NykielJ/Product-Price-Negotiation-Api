using ProductPriceNegotiationApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductPriceNegotiationApi.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
    }
}
