namespace OpenExchangeRates;

/// <summary>
///     Represents the response received from the Open Exchange Rates API when querying usage data.
/// </summary>
/// <remarks>
///     This class encapsulates the status of the API response and the associated usage data.
/// </remarks>
public sealed class UsageResponse
{
    /// <summary>
    ///     Gets the status code of the API response.
    /// </summary>
    /// <remarks>
    ///     The status code indicates the result of the API request. It can be used to determine
    ///     whether the request was successful or if an error occurred.
    /// </remarks>
    public ushort Status { get; init; }

    /// <summary>
    ///     Gets the usage data associated with the response from the Open Exchange Rates API.
    /// </summary>
    /// <value>
    ///     An instance of <see cref="OpenExchangeRates.UsageData" /> containing details about the application ID,
    ///     API status, subscription plan, and usage statistics.
    /// </value>
    /// <remarks>
    ///     This property is required and provides detailed information about the API usage, including the current
    ///     subscription plan and usage limits.
    /// </remarks>
    public required UsageData Data { get; init; }
}