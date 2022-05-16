using System.Text.Json.Serialization;

namespace OpenExchangeRates;

public sealed class ConvertResponse
{
    public ConvertRequest Request { get; set; } = null!;

    [JsonPropertyName("response")]
    public decimal Amount { get; set; }

    [JsonPropertyName("meta")]
    public ConversionMetadata Metadata { get; set; } = null!;
}