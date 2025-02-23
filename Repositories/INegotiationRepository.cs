using ProductPriceNegotiationApi.Models;
using System.Threading.Tasks;

namespace ProductPriceNegotiationApi.Repositories
{
    public interface INegotiationRepository
    {
        Task AddAsync(Negotiation negotiation);
        Task<Negotiation?> GetByProductIdAsync(int productId);
        Task RemoveAsync(Negotiation negotiation);
    }
}
