namespace OpenExchangeRates
{
    public sealed class UsageResponse
    {
        public ushort Status { get; set; }

        public UsageData Data { get; set; }
    }
}