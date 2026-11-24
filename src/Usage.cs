using System.Text.Json.Serialization;

namespace OpenExchangeRates;

/// <summary>
///     Represents the usage statistics for the Open Exchange Rates API, including details about
///     the number of requests made, the quota, remaining requests, and time-related metrics.
/// </summary>
public sealed class Usage
{
    /// <summary>
    ///     Gets the total number of API requests made within the current usage period.
    /// </summary>
    public int Requests { get; init; }

    /// <summary>
    ///     Gets the maximum number of API requests allowed within the current billing period.
    /// </summary>
    /// <remarks>
    ///     This property represents the quota of API requests allocated to the application
    ///     for the current billing cycle. It is useful for monitoring and managing API usage
    ///     to ensure the application stays within its allowed limits.
    /// </remarks>
    [JsonPropertyName("requests_quota")]
    public int RequestsQuota { get; init; }

    /// <summary>
    ///     Gets the number of API requests remaining within the current quota period.
    /// </summary>
    [JsonPropertyName("requests_remaining")]
    public int RequestsRemaining { get; init; }

    /// <summary>
    ///     Gets the number of days that have elapsed in the current billing period for the Open Exchange Rates API usage.
    /// </summary>
    [JsonPropertyName("days_elapsed")]
    public int DaysElapsed { get; init; }

    /// <summary>
    ///     Gets the number of days remaining in the current billing period for the Open Exchange Rates API.
    /// </summary>
    /// <remarks>
    ///     This property indicates how many days are left before the current billing cycle ends.
    /// </remarks>
    [JsonPropertyName("days_remaining")]
    public int DaysRemaining { get; init; }

    /// <summary>
    ///     Gets the average number of API requests made per day.
    /// </summary>
    /// <remarks>
    ///     This property provides a calculated value representing the average number of requests
    ///     made to the Open Exchange Rates API on a daily basis.
    /// </remarks>
    [JsonPropertyName("daily_average")]
    public int DailyAverage { get; init; }
}