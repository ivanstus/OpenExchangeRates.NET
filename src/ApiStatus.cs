namespace OpenExchangeRates;

/// <summary>
///     Represents the status of the API in the Open Exchange Rates system.
/// </summary>
/// <summary>
///     The API status is unknown or not specified.
/// </summary>
/// <summary>
///     The API is active and operational.
/// </summary>
/// <summary>
///     Access to the API is restricted.
/// </summary>
public enum ApiStatus
{
    /// <summary>
    ///     Represents an unknown or unspecified status of the API in the Open Exchange Rates system.
    /// </summary>
    Unknown,

    /// <summary>
    ///     Indicates that the API is active and operational.
    /// </summary>
    Active,

    /// <summary>
    ///     Indicates that access to the API is restricted, typically due to insufficient permissions
    ///     or exceeding the allocated usage limits.
    /// </summary>
    AccessRestricted
}