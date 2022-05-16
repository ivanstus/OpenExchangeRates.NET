using System.Text.Json.Serialization;

namespace OpenExchangeRates;

public sealed class Plan
{
    public string Name { get; set; } = null!;
    public string Quota { get; set; } = null!;

    [JsonPropertyName("update_frequency")]
    public string UpdateFrequency { get; set; } = null!;

    public PlanFeatures Features { get; set; } = null!;
}