using ProductPriceNegotiationApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductPriceNegotiationApi.Data
{
    public class InMemoryNegotiationRepository
    {
        private readonly List<Negotiation> _negotiations = new List<Negotiation>();

        public void Add(Negotiation negotiation)
        {
            _negotiations.Add(negotiation);
        }

        public Negotiation GetByProductId(int productId)
        {
            return _negotiations.FirstOrDefault(n => n.ProductId == productId);
        }

        public IEnumerable<Negotiation> GetAll()
        {
            return _negotiations;
        }
    }
}
