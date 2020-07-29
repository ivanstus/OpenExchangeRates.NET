using System.Text.Json.Serialization;

namespace OpenExchangeRates
{
    public sealed class PlanFeatures
    {
        public bool Base { get; set; }

        public bool Symbols { get; set; }

        public bool Experimental { get; set; }

        [JsonPropertyName("time-series")]
        public bool TimeSeries { get; set; }

        public bool Convert { get; set; }
    }
}