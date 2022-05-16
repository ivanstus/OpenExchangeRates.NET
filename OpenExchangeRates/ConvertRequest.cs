namespace OpenExchangeRates;

public sealed class ConvertRequest
{
    public string Query { get; set; } = null!;
    public decimal Amount { get; set; }
    public string From { get; set; } = null!;
    public string To { get; set; } = null!;
}