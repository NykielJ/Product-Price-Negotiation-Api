using ProductPriceNegotiationApi.Models;
using ProductPriceNegotiationApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductPriceNegotiationApi.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task AddProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.Name))
                throw new ArgumentException("Product name cannot be empty.");

            if (product.Price <= 0)
                throw new ArgumentException("Price must be greater than 0.");

            await _productRepository.AddAsync(product);
        }

        public async Task<Product?> GetProduct(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllAsync();
        }
    }
}
