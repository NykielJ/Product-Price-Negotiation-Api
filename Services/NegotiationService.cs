using ProductPriceNegotiationApi.Data;
using ProductPriceNegotiationApi.Models;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProductPriceNegotiationApi.Services
{
    public class NegotiationService
    {
        private readonly AppDbContext _context;

        public NegotiationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task StartNegotiation(int productId, decimal proposedPrice)
        {
            var product = await _context.Products.FindAsync(productId);
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

            _context.Negotiations.Add(negotiation);
            await _context.SaveChangesAsync();
        }

        public async Task RespondToNegotiation(int productId, bool accept)
        {
            var negotiation = await _context.Negotiations.FirstOrDefaultAsync(n => n.ProductId == productId);
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

            await _context.SaveChangesAsync();
        }
    }
}
