using System.Text.Json.Serialization;

namespace OpenExchangeRates;

/// <summary>
///     Represents the response from the Open Exchange Rates API containing exchange rate data.
/// </summary>
/// <remarks>
///     This class provides information about exchange rates, including the timestamp of the data,
///     the base currency used for the rates, and a collection of rates for various currencies.
/// </remarks>
public sealed class RatesResponse
{
    /// <summary>
    ///     Gets or sets the timestamp indicating when the exchange rate data was last updated.
    /// </summary>
    /// <remarks>
    ///     The timestamp is represented as a <see cref="DateTimeOffset" /> and reflects the time
    ///     the data was retrieved from the Open Exchange Rates API.
    /// </remarks>
    public DateTimeOffset Timestamp { get; init; }

    /// <summary>
    ///     Gets the base currency used for the exchange rates in the response.
    /// </summary>
    /// <remarks>
    ///     The base currency is the reference currency against which all other currency exchange rates
    ///     are calculated. For example, if the base currency is "USD", the rates represent the value
    ///     of one unit of each currency in terms of USD.
    /// </remarks>
    [JsonPropertyName("base")]
    public required string BaseCurrency { get; init; }

    /// <summary>
    ///     Gets the collection of exchange rates for various currencies.
    /// </summary>
    /// <remarks>
    ///     The keys in the dictionary represent the currency codes (e.g., "USD", "EUR"),
    ///     and the values represent the exchange rates relative to the base currency.
    /// </remarks>
    public required IReadOnlyDictionary<string, decimal> Rates { get; init; }
}