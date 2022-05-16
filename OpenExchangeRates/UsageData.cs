using System.Text.Json.Serialization;

namespace OpenExchangeRates;

public sealed class UsageData
{
    [JsonPropertyName("app_id")]
    public string AppId { get; set; } = null!;

    public ApiStatus Status { get; set; }
    public Plan Plan { get; set; } = null!;
    public Usage Usage { get; set; } = null!;
}