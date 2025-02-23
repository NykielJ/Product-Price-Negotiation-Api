using Microsoft.EntityFrameworkCore;
using ProductPriceNegotiationApi.Data;
using ProductPriceNegotiationApi.Models;
using System.Threading.Tasks;

namespace ProductPriceNegotiationApi.Repositories
{
    public class NegotiationRepository : INegotiationRepository
    {
        private readonly AppDbContext _context;

        public NegotiationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Negotiation negotiation)
        {
            _context.Negotiations.Add(negotiation);
            await _context.SaveChangesAsync();
        }

        public async Task<Negotiation?> GetByProductIdAsync(int productId)
        {
            return await _context.Negotiations.FirstOrDefaultAsync(n => n.ProductId == productId);
        }

        public async Task RemoveAsync(Negotiation negotiation)
        {
            _context.Negotiations.Remove(negotiation);
            await _context.SaveChangesAsync();
        }
    }
}
