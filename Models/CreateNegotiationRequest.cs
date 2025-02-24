using System.ComponentModel.DataAnnotations;

namespace ProductPriceNegotiationApi.Models
{
    public class CreateNegotiationRequest
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Proposed price must be greater than 0.")]
        public decimal ProposedPrice { get; set; }
    }
}
