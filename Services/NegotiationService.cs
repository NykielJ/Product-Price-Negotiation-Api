using ProductPriceNegotiationApi.Models;
using ProductPriceNegotiationApi.Repositories;
using System;
using System.Threading.Tasks;

namespace ProductPriceNegotiationApi.Services
{
    public class NegotiationService
    {
        private readonly INegotiationRepository _negotiationRepository;
        private readonly IProductRepository _productRepository;

        public NegotiationService(INegotiationRepository negotiationRepository, IProductRepository productRepository)
        {
            _negotiationRepository = negotiationRepository;
            _productRepository = productRepository;
        }

        public async Task StartNegotiation(int productId, decimal proposedPrice)
        {
            var product = await _productRepository.GetByIdAsync(productId);
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

            await _negotiationRepository.AddAsync(negotiation);
        }

        public async Task RespondToNegotiation(int productId, bool accept)
        {
            var negotiation = await _negotiationRepository.GetByProductIdAsync(productId);
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
            }

            await _negotiationRepository.AddAsync(negotiation);
        }
    }
}
