namespace OpenExchangeRates;

public sealed class ConversionMetadata
{
    public DateTimeOffset Timestamp { get; set; }
    public decimal Rate { get; set; }
}