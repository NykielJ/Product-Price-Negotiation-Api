using ProductPriceNegotiationApi.Models;
using System.Threading.Tasks;

namespace ProductPriceNegotiationApi.Repositories
{
    public interface INegotiationRepository
    {
        Task AddAsync(Negotiation negotiation);
        Task<Negotiation?> GetByIdAsync(int negotiationId);
        Task<Negotiation?> GetByProductIdAsync(int productId);
        Task RemoveAsync(Negotiation negotiation);
        Task UpdateAsync(Negotiation negotiation);
    }
}
