namespace OpenExchangeRates;

/// <summary>
///     Represents metadata related to a currency conversion operation,
///     including details such as the timestamp of the conversion and the exchange rate used.
/// </summary>
public sealed class ConversionMetadata
{
    /// <summary>
    ///     Gets the timestamp indicating when the currency conversion operation occurred.
    /// </summary>
    public DateTimeOffset Timestamp { get; init; }

    /// <summary>
    ///     Gets the exchange rate used for the currency conversion.
    /// </summary>
    /// <value>
    ///     A <see cref="decimal" /> representing the exchange rate applied during the conversion.
    /// </value>
    public decimal Rate { get; init; }
}