using ProductPriceNegotiationApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductPriceNegotiationApi.Data
{
    public class InMemoryProductRepository
    {
        private readonly List<Product> _products = new List<Product>();

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public Product Get(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }
    }
}
