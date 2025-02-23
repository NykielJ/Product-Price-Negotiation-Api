using System.ComponentModel.DataAnnotations;

namespace ProductPriceNegotiationApi.Models
{
    public class Negotiation
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Proposed price must be greater than zero.")]
        public decimal ProposedPrice { get; set; }

        public bool IsAccepted { get; set; }
        public int Attempts { get; set; }
        public DateTime DateProposed { get; set; }
        public DateTime? LastAttemptDate { get; set; }
    }
}
