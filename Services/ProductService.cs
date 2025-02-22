using ProductPriceNegotiationApi.Data;
using ProductPriceNegotiationApi.Models;
using System.Collections.Generic;
using ProductPriceNegotiationApi.Data;
using ProductPriceNegotiationApi.Models;

namespace ProductPriceNegotiationApi.Services
{
    public class ProductService
    {
        private readonly InMemoryProductRepository _productRepository;

        public ProductService(InMemoryProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void AddProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.Name))
                throw new ArgumentException("Product name cannot be empty.");
            
            if (product.Price <= 0)
                throw new ArgumentException("Price must be greater than 0.");
            
            _productRepository.Add(product);
        }

        public Product GetProduct(int id)
        {
            return _productRepository.Get(id);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }
    }
}
