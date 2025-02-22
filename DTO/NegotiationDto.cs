namespace ProductPriceNegotiationApi.DTOs
{
    public class NegotiationDto
    {
        public int ProductId { get; set; }
        public decimal ProposedPrice { get; set; }
        public bool IsAccepted { get; set; }
        public int Attempts { get; set; }
        public DateTime DateProposed { get; set; }
    }
}
