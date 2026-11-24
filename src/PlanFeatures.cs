using System.Text.Json.Serialization;

namespace OpenExchangeRates;

/// <summary>
///     Represents the features available in a subscription plan of the Open Exchange Rates API.
/// </summary>
public sealed class PlanFeatures
{
    /// <summary>
    ///     Gets or sets a value indicating whether the base currency feature is available in the subscription plan.
    /// </summary>
    public bool Base { get; init; }

    /// <summary>
    ///     Gets or sets a value indicating whether the subscription plan includes support for currency symbols.
    /// </summary>
    public bool Symbols { get; init; }

    /// <summary>
    ///     Gets or sets a value indicating whether the experimental features are enabled
    ///     for the subscription plan of the Open Exchange Rates API.
    /// </summary>
    public bool Experimental { get; init; }

    /// <summary>
    ///     Gets or sets a value indicating whether the time series feature is available in the subscription plan.
    /// </summary>
    /// <remarks>
    ///     The time series feature allows access to historical exchange rate data over a specified period.
    /// </remarks>
    [JsonPropertyName("time-series")]
    public bool TimeSeries { get; init; }

    /// <summary>
    ///     Gets or sets a value indicating whether the subscription plan includes the ability to convert currency values.
    /// </summary>
    public bool Convert { get; init; }
}