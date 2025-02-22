namespace ProductPriceNegotiationApi.Utilities
{
    public static class ValidationHelper
    {
        public static void ValidateProductName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Product name cannot be empty.");
        }

        public static void ValidateProductPrice(decimal price)
        {
            if (price <= 0)
                throw new ArgumentException("Price must be greater than 0.");
        }

        public static void ValidateProposedPrice(decimal proposedPrice)
        {
            if (proposedPrice <= 0)
                throw new ArgumentException("Proposed price must be greater than 0.");
        }

        public static void ValidateAttempts(int attempts)
        {
            if (attempts > 3)
                throw new InvalidOperationException("Maximum negotiation attempts reached.");
        }
    }
}
