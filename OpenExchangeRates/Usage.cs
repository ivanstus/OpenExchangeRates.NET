using System.Text.Json.Serialization;

namespace OpenExchangeRates;

public sealed class Usage
{
    public int Requests { get; set; }

    [JsonPropertyName("requests_quota")]
    public int RequestsQuota { get; set; }

    [JsonPropertyName("requests_remaining")]
    public int RequestsRemaining { get; set; }

    [JsonPropertyName("days_elapsed")]
    public int DaysElapsed { get; set; }

    [JsonPropertyName("days_remaining")]
    public int DaysRemaining { get; set; }

    [JsonPropertyName("daily_average")]
    public int DailyAverage { get; set; }
}