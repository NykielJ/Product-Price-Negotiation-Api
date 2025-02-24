using System;
using System.ComponentModel.DataAnnotations;

namespace ProductPriceNegotiationApi.Models
{
    public class Negotiation
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Proposed price must be greater than 0.")]
        public decimal ProposedPrice { get; set; }

        public bool IsAccepted { get; set; }
        public int Attempts { get; set; } = 0;
        public DateTime DateProposed { get; set; } = DateTime.UtcNow;
        public DateTime? LastAttemptDate { get; set; }
        
        public bool IsExpired => LastAttemptDate.HasValue && (DateTime.UtcNow - LastAttemptDate.Value).Days > 7;
    }
}
