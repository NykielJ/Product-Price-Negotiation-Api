using ProductPriceNegotiationApi.Data;
using ProductPriceNegotiationApi.Models;
using System;

namespace ProductPriceNegotiationApi.Services
{
    public class NegotiationService
    {
        private readonly InMemoryNegotiationRepository _negotiationRepository;
        private readonly InMemoryProductRepository _productRepository;

        public NegotiationService(InMemoryNegotiationRepository negotiationRepository, InMemoryProductRepository productRepository)
        {
            _negotiationRepository = negotiationRepository;
            _productRepository = productRepository;
        }

        public void StartNegotiation(int productId, decimal proposedPrice)
        {
            var product = _productRepository.Get(productId);
            if (product == null)
                throw new ArgumentException("Product not found.");

            if (proposedPrice <= 0)
                throw new ArgumentException("Proposed price must be greater than 0.");

            var negotiation = new Negotiation
            {
                ProductId = productId,
                ProposedPrice = proposedPrice,
                DateProposed = DateTime.Now,
                Attempts = 0
            };

            _negotiationRepository.Add(negotiation);
        }

        public void RespondToNegotiation(int productId, bool accept)
        {
            var negotiation = _negotiationRepository.GetByProductId(productId);
            if (negotiation == null)
                throw new ArgumentException("Negotiation not found.");

            if (accept)
            {
                negotiation.IsAccepted = true;
            }
            else
            {
                if (negotiation.Attempts >= 3)
                    throw new InvalidOperationException("Maximum negotiation attempts reached.");

                negotiation.Attempts++;
                negotiation.LastAttemptDate = DateTime.Now;

                if (negotiation.LastAttemptDate.HasValue && (DateTime.Now - negotiation.LastAttemptDate.Value).Days > 7)
                {
                    throw new InvalidOperationException("Negotiation canceled after 7 days without new proposal.");
                }
            }
        }
    }
}
