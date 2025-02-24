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

        public async Task<int> StartNegotiation(int productId, decimal proposedPrice)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new ArgumentException("Product not found.");

            if (proposedPrice <= 0)
                throw new ArgumentException("Proposed price must be greater than 0.");

            var existingNegotiation = await _negotiationRepository.GetByProductIdAsync(productId);

            if (existingNegotiation != null)
            {
                if (existingNegotiation.IsAccepted)
                    throw new InvalidOperationException("This negotiation has already been accepted.");

                if (existingNegotiation.IsExpired)
                {
                    await _negotiationRepository.RemoveAsync(existingNegotiation);
                    existingNegotiation = null;
                }
                else if (existingNegotiation.Attempts >= 3)
                {
                    throw new InvalidOperationException("Maximum negotiation attempts reached.");
                }
            }

            var newNegotiation = new Negotiation
            {
                ProductId = productId,
                ProposedPrice = proposedPrice,
                DateProposed = DateTime.UtcNow,
                Attempts = existingNegotiation?.Attempts + 1 ?? 1
            };

            await _negotiationRepository.AddAsync(newNegotiation);
            return newNegotiation.Id;
        }

        public async Task UpdateNegotiation(int negotiationId, decimal proposedPrice)
        {
            var negotiation = await _negotiationRepository.GetByIdAsync(negotiationId);
            if (negotiation == null)
                throw new ArgumentException("Negotiation not found.");

            if (negotiation.IsAccepted)
                throw new InvalidOperationException("This negotiation has already been accepted.");

            if (negotiation.IsExpired)
            {
                await _negotiationRepository.RemoveAsync(negotiation);
                throw new InvalidOperationException("Negotiation has expired and has been removed.");
            }

            if (negotiation.Attempts >= 3)
                throw new InvalidOperationException("Maximum negotiation attempts reached.");

            negotiation.ProposedPrice = proposedPrice;
            negotiation.DateProposed = DateTime.UtcNow;
            negotiation.Attempts++;

            await _negotiationRepository.UpdateAsync(negotiation);
        }

        public async Task<Negotiation?> GetNegotiationById(int negotiationId)
        {
            var negotiation = await _negotiationRepository.GetByIdAsync(negotiationId);

            if (negotiation != null && negotiation.IsExpired)
            {
                await _negotiationRepository.RemoveAsync(negotiation);
                return null;
            }

            return negotiation;
        }

        public async Task RespondToNegotiation(int negotiationId, bool accept)
        {
            var negotiation = await _negotiationRepository.GetByIdAsync(negotiationId);
            if (negotiation == null)
                throw new ArgumentException("Negotiation not found.");

            if (negotiation.IsExpired)
            {
                await _negotiationRepository.RemoveAsync(negotiation);
                throw new InvalidOperationException("This negotiation has expired and has been removed.");
            }

            if (negotiation.Attempts >= 3)
                throw new InvalidOperationException("Maximum negotiation attempts reached.");

            if (accept)
            {
                negotiation.IsAccepted = true;
            }
            else
            {
                negotiation.Attempts++;
                negotiation.LastAttemptDate = DateTime.UtcNow;
            }

            await _negotiationRepository.UpdateAsync(negotiation);
        }
    }
}
