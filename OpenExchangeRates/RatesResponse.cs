﻿using System.Text.Json.Serialization;

namespace OpenExchangeRates;

public sealed class RatesResponse
{
    public DateTimeOffset Timestamp { get; set; }

    [JsonPropertyName("base")]
    public string BaseCurrency { get; set; } = null!;

    public IReadOnlyDictionary<string, decimal> Rates { get; set; } = null!;
}