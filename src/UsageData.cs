using System.Text.Json.Serialization;

namespace OpenExchangeRates;

/// <summary>
///     Represents the usage data for the Open Exchange Rates API, including details about the application ID,
///     API status, subscription plan, and usage statistics.
/// </summary>
public sealed class UsageData
{
    /// <summary>
    ///     Gets the application ID associated with the Open Exchange Rates API.
    ///     This ID is required for authenticating API requests.
    /// </summary>
    [JsonPropertyName("app_id")]
    public required string AppId { get; init; }

    /// <summary>
    ///     Gets the current status of the Open Exchange Rates API.
    /// </summary>
    /// <remarks>
    ///     The status indicates whether the API is active, restricted, or in an unknown state.
    /// </remarks>
    public ApiStatus Status { get; init; }

    /// <summary>
    ///     Gets the subscription plan associated with the current application ID.
    /// </summary>
    /// <remarks>
    ///     The <see cref="Plan" /> object provides details about the subscription plan,
    ///     including its name, quota, update frequency, and available features.
    /// </remarks>
    public required Plan Plan { get; init; }

    /// <summary>
    ///     Gets the usage statistics for the Open Exchange Rates API, including details such as the number of requests made,
    ///     the quota of requests allowed, the remaining requests, and time-related statistics like days elapsed and remaining.
    /// </summary>
    public required Usage Usage { get; init; }
}