using System.Text.Json.Serialization;

namespace OpenExchangeRates;

/// <summary>
///     Represents a subscription plan in the Open Exchange Rates API, including details about the plan's name,
///     quota, update frequency, and available features.
/// </summary>
public sealed class Plan
{
    /// <summary>
    ///     Gets the name of the subscription plan. This property represents the unique identifier or title
    ///     of the plan as provided by the Open Exchange Rates API.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Gets the quota associated with the subscription plan, which typically represents the maximum
    ///     number of API requests or data usage allowed within a specific period.
    /// </summary>
    public required string Quota { get; init; }

    /// <summary>
    ///     Gets the update frequency of the subscription plan, indicating how often the exchange rates are updated.
    /// </summary>
    [JsonPropertyName("update_frequency")]
    public required string UpdateFrequency { get; init; }

    /// <summary>
    ///     Gets the features available in the subscription plan, such as support for base currency changes,
    ///     symbol availability, experimental features, time-series data, and currency conversion.
    /// </summary>
    public required PlanFeatures Features { get; init; }
}