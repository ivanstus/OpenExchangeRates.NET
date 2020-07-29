using System.Text.Json.Serialization;

namespace OpenExchangeRates
{
    public sealed class UsageData
    {
        [JsonPropertyName("app_id")]
        public string AppId { get; set; }

        public ApiStatus Status { get; set; }

        public Plan Plan { get; set; }

        public Usage Usage { get; set; }
    }
}