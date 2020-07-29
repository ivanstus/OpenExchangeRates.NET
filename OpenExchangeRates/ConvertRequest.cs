namespace OpenExchangeRates
{
    public sealed class ConvertRequest
    {
        public string Query { get; set; }

        public decimal Amount { get; set; }

        public string From { get; set; }

        public string To { get; set; }
    }
}