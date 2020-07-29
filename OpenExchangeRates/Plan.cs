using System.Text.Json.Serialization;

namespace OpenExchangeRates
{
    public sealed class Plan
    {
        public string Name { get; set; }

        public string Quota { get; set; }

        [JsonPropertyName("update_frequency")]
        public string UpdateFrequency { get; set; }

        public PlanFeatures Features { get; set; }
    }
}